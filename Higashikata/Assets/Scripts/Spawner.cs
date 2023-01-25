using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private bool _end = true;
    public bool End => _end;

    private float origin;
    private GameObject projectile;
    private List<float> angles;
    private float shootingTime;
    private float speed;
    private int indice_angle = 0;
    private Quaternion init;
    private float angle;
    private Quaternion target;
    private bool isInstant = false;
    private float totalDistance = 0.0f;
    private float latence = 0.0f;
    private bool notwaiting = false;

    void Start()
    {
        init = Quaternion.Euler(0, origin, 0);
        transform.rotation = init;

        //Maj de la valeur des angles si l'origin n'est pas en 0
        if (origin != 0)
        {
            for (int i = 0; i<angles.Count; i++)
            {
                var angle = angles[i];
                var newAngle = angle + origin;
                angles[i] = newAngle;
            }
        }
        
        //Fonction qui permet de gérer les angles de valeur élevée en ajoutant des angles intermédiaires
        ConvertAngles();
        
        angle = angles[0];
        target = Quaternion.Euler(0, angle, 0);
        
        //Calcul de la distance totale
        totalDistance = angle;
        for (int i = 0; i < angles.Count-1; i++)
        {
            totalDistance += Mathf.Abs(angles[i+1] - angles[i]);
        }

        //Si on tire tout en même temps
        if (isInstant)
        {
            for (int k = 0; k < angles.Count; k++)
            {
                //On précalcule le nombre de projectiles en fonction de la distance à parcourir d'un angle à l'autre
                var distance = Mathf.Abs(origin - angle);
                int n = Mathf.FloorToInt(distance * Time.fixedDeltaTime / shootingTime);
                
                //On instantie ensuite les projectiles
                if (n > 0)
                {
                    for (int i = 0; i <= n; i++)
                    { 
                        //Instantiate(projectile, transform.position, transform.rotation); 
                        ObjectPooler.Instance.Spawn(projectile.name, transform.position, transform.rotation);
                        transform.rotation = Quaternion.Slerp(init, target, (float) i / n);
                    }
                }
                origin = angle;
                init = Quaternion.Euler(0,angle,0);
                if (k < angles.Count - 1)
                {
                    angle = angles[k+1];
                    target = Quaternion.Euler(0,angle,0);
                }
                
                
            }
            _end = true;
            transform.gameObject.SetActive(false);
        }
        else
        {
            //Coroutine qui va simplement instantier un projectile à la fois jusqu'à ce qu'elle soit arrêtée par un StopCoroutine
            StartCoroutine(SpawnProjectile(projectile,shootingTime));
        }

    }
    
    private IEnumerator SpawnProjectile(GameObject projectile, float time)
    {
        yield return new WaitForSeconds(latence);
        notwaiting = true;

        while (true)
        {
            var proj = ObjectPooler.Instance.Spawn(projectile.name, transform.position, transform.rotation);
            yield return new WaitForSeconds(time);
        }
    }

    //Méthode qui set les paramètres de la classe
    public void Set(float _origin, GameObject _projectile, List<float> _angles, float _shootingTime, float _speed, bool instant, float l)
    {
        origin = _origin;
        projectile = _projectile;
        angles = new List<float>(_angles);
        shootingTime = _shootingTime;
        speed = _speed;
        isInstant = instant;
        latence = l;
    }

    public void StartShoot()
    {
        _end = false;
    }

    //Méthode qui ajoute des angles intermédiaires dans le cas où l'on met des valeurs de plus de 90°. En effet sans ces valeurs de transitions, des bugs peuvent apparaître vu la façon dont les angles sont gérés en interne par Unity
    private void ConvertAngles()
    {
        List<float> newAngles = new List<float>();
        var sAngle = origin;

        for (int i = 0; i < angles.Count; i++)
        {
            var goal = angles[i];
            var distance = Mathf.Abs(sAngle - goal);
            while (distance > 90)
            {
                int indicateur = (goal - sAngle > 0 ? 1 : -1);
                newAngles.Add(sAngle + 90 * indicateur);
                sAngle += indicateur * 90;
                distance = Mathf.Abs(sAngle - goal);
            }
            newAngles.Add(goal);
            sAngle = goal;
        }
        angles = newAngles;
    }

    public void Update()
    {
        if (notwaiting)
        {
            if (_end)
            {
                transform.gameObject.SetActive(false);
            }
            else
            {
                if (indice_angle > angles.Count)
                {
                    _end = true;
                }
                else
                {
                    var step = Time.deltaTime * speed * 10;
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, target, step);
                    var distance = Mathf.Abs(transform.rotation.eulerAngles.y - angle) + 0.001;

                    if (distance % 360 <= 0.01)
                    {
                        indice_angle++;
                        init = transform.rotation;
                        if (indice_angle < angles.Count)
                        {
                            angle = angles[indice_angle];
                            target = Quaternion.Euler(0, angle, 0);
                        }
                    }
                }
            }
        }
        
    }
}
