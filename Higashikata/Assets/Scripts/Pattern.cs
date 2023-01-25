using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pattern : MonoBehaviour
{
    [SerializeField] private List<PatternData> patterns;
    [SerializeField] private GameObject spawner;
    [SerializeField] private bool cycle = false;
    [SerializeField] private float timeBetweenPatterns = 1;
    [SerializeField] private int poolSize = 250;
    [SerializeField] private bool randomize = false;
    private List<GameObject> activeSpanwers = new List<GameObject>();

    public List<PatternData> Patterns
    {
        set => patterns = value;
    }
    
    
    public void Start()
    {
        foreach (var pattern in patterns)
        {
            var projectiles = pattern.Projectiles;
            foreach (var proj in projectiles)
            {
                //Ajout des pools d'objets pour chaque pattern
                ObjectPooler.Instance.AddPool(new ObjectPooler.Pool(proj.name, proj, poolSize));
            }
        }
        
        ObjectPooler.Instance.Init();

        //StartCoroutine(StartPatterns());
    }

    private IEnumerator StartPatterns()
    {
        do
        {
            //Si l'option randomize n'est pas cochée, on fait les patterns dans leur ordre d'ajout
            if (!randomize)
            {
                foreach (var pattern in patterns)
                {
                    yield return new WaitForSeconds(timeBetweenPatterns);
                    StartCoroutine(DoPattern(pattern));
                    yield return new WaitUntil(() => CheckEndSpawner());
                }
            }
            else
            {
                List<PatternData> subpatterns = new List<PatternData>(patterns);
                while (subpatterns.Count != 0)
                {
                    yield return new WaitForSeconds(timeBetweenPatterns);
                    int i = Random.Range(0,subpatterns.Count);
                    StartCoroutine(DoPattern(subpatterns[i]));
                    subpatterns.Remove(subpatterns[i]);
                    //yield return new WaitUntil(() => goNext);
                    yield return new WaitUntil(() => CheckEndSpawner());
                }
            }
        } while (cycle);
        
        yield return null;
    }
    
    //Méthode qui lance un pattern donné en paramètre
    private IEnumerator DoPattern(PatternData pattern)
    {
        //Init des valeurs nécessaires pour chaque pattern
        var angles = pattern.Angles; //Liste de liste d'angles qui correspondent aux rotations effectuées par un tir
        var speed = pattern.Speed; //Vitesse de rotation pour un tir
        var originsPoints = pattern.Origins; //Valeur d'un angle correspondant à un point d'origine de tir
        var projectiles = pattern.Projectiles; //Liste des projectiles utilisées
        var shootingTime = pattern.ShootingTime; //Temps entre chaque tir de projectile
        var isShoot = pattern.TirEnMemeTemps; //Booléen qui définit si on tir tout en même temps ou progressivement
        var latences = pattern.Latences; //éventuelles latences entre les spawners
        
        //Création d'une liste de latences de base si non définie
        if (latences == null || latences.Count == 0)
        {
            latences = new List<float>();
            latences.Add(0);
        }

        //On itère ensuite sur le nombre de points d'origines, ie le nombres de tirs en même temps
        for (int i = 0; i < originsPoints.Count; i++)
        {
            //Pour chacune, on instantie un spawner
            var entity = Instantiate(spawner, transform.position, transform.rotation);
            entity.transform.SetParent(transform);
            activeSpanwers.Add(entity);
            var spawn = entity.GetComponent<Spawner>();
            //On set ses paramètres avec les valeurs adéquates
            spawn.Set(originsPoints[i], i >= projectiles.Count ? projectiles[projectiles.Count - 1] : projectiles[i] , i >= angles.Count ? angles[angles.Count - 1].list : angles[i].list, shootingTime, speed, isShoot, (i >= latences.Count ? latences[latences.Count - 1] : latences[i]));
            //On démarre son tir
            spawn.StartShoot();
        }

        yield return null;
    }
    
    public void StartShoot()
    {
        StartCoroutine(StartPatterns());
    }

    //Méthode qui vérifie si tous les spawners actifs ont terminé de tirer
    //Dans l'idéal, il faudrait changer cette façon de faire car elle est appelée à chaque frame d'exécution, mais c'est le mieux que j'ai trouvé pour attendre que tous les spawners aient terminé leur action avant de passer au pattern suivant
    private bool CheckEndSpawner()
    {
        foreach (var sp in activeSpanwers)
        {
            if (!sp.GetComponent<Spawner>().End)
            {
                return false;
            }
        }

        foreach (var sp in activeSpanwers)
        {
            Destroy(sp); 
        }

        activeSpanwers = new List<GameObject>();
        
        return true;
    }
}
