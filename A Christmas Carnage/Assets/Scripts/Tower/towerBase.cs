using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/*Fonctionnement de base de la tour*/
public class towerBase : MonoBehaviour
{
    [SerializeField] private float _rad = 10;
    [SerializeField] private float _shootingSpeedBasicTower;
    [SerializeField] private float _shootingSpeedAOETower;
    [SerializeField] private float _shootingSpeedFireTower;

    private List<GameObject> ennemies = new List<GameObject>();
    private SphereCollider _sphereCollider; 
    public GameObject _projectile;
    public GameObject _projectileFire;
    public GameObject _projectileAOE;
    public GameObject _createZone;
    [SerializeField] private float _createRad = 3;

    [SerializeField] private int _value; 
    
    [SerializeField] private int _level = 1;
    private bool isFireTower = false;
    private bool isAOETower = false;

    private AudioSource audioData;
    private static float _volume;
    // Start is called before the first frame update
    void Start()
    {
        /*On récupère le Collider de notre objet pour agir dessus dans le script*/
        _sphereCollider = gameObject.GetComponent<SphereCollider>();
        /*On vérifie que notre Collider est bien en isTrigger*/
        _sphereCollider.isTrigger = true;

        /*On démarre le shooting*/
        StartCoroutine("shooting");

        /*On récupère notre Component pour le son*/
        audioData = GetComponent<AudioSource>();
        audioData.volume = _volume;
    }

    // Update is called once per frame
    void Update()
    {
        /*On peut modifier le radius du Sphere Collider avec le paramètre _rad*/
        _sphereCollider.radius = _rad;
        /*On peut modifier le radius de création de tour*/
        _createZone.GetComponent<SphereCollider>().radius = _createRad;
    }

    public void Upgrade()
    {
        _level++;
        _shootingSpeedAOETower = (float)4.0f/((float)_level);
        _shootingSpeedBasicTower = (float)1.0f/(float)_level;
        _shootingSpeedFireTower = (float)5.0f/(float)_level;
    }
    

    /*On détecte les ennemis qui rentrent dans la zone de la tour
    Cette zone est délimitée par le Collider */
    void OnTriggerEnter(Collider entity){
        var currentEnnemy = entity.gameObject;
        if (currentEnnemy.tag == "ennemy") {
            ennemies.Add(currentEnnemy);
        }
    }

    /*On retire de notre liste de détection les ennemis qui sortent de la zone de la tour*/
    void OnTriggerExit(Collider entity){
        var currentEnnemy = entity.gameObject;
        if (currentEnnemy.tag == "ennemy") {
            ennemies.Remove(currentEnnemy);
        }
    }

/*Coroutine qui gère le tire de la tour*/
    IEnumerator shooting(){
        
        while(true){
            if (isFireTower){
                yield return new WaitForSeconds(_shootingSpeedFireTower);
            }
            else if (isAOETower){
                yield return new WaitForSeconds(_shootingSpeedAOETower);
            }
            else {
                yield return new WaitForSeconds(_shootingSpeedBasicTower);
            }
            /*Si l'ennemi focus est null, on le retire de la liste de focus*/
            if (ennemies.Count != 0 && ennemies[0] == null) {
                ennemies.Remove(ennemies[0]);
            }
            /*Sinon on le récupère pour lui tirer dessus*/
            if (ennemies.Count != 0 && ennemies[0] != null) {
                
                var currentEnnemy = ennemies[0];        

                /*On instantie un missile et on set son focus sur l'ennemi actuel*/
                GameObject actualProjectile = (isFireTower ? _projectileFire : (isAOETower ? _projectileAOE : _projectile));

                var _missile = UnityEngine.Object.Instantiate(actualProjectile,transform.position, transform.rotation);
                _missile.GetComponent<projectileBehavior>().setFocus(currentEnnemy);

                /*Si la tour est en mode AOE, on recrée un missile pour deux cibles supplémentaires si possible, ie si 3 ennemis peuvent êtres focus*/
                if (isAOETower && ennemies.Count >= 2 &&ennemies[1] != null){
                    var _missileBis = UnityEngine.Object.Instantiate(actualProjectile,transform.position, transform.rotation);
                    _missileBis.GetComponent<projectileBehavior>().setFocus(ennemies[1]);
                }
                if (isAOETower && ennemies.Count >= 3 && ennemies[2] != null){
                    var _missileTer = UnityEngine.Object.Instantiate(actualProjectile,transform.position, transform.rotation);
                    _missileTer.GetComponent<projectileBehavior>().setFocus(ennemies[2]);
                }

                /*On joue un son quand on tire*/
                audioData.Play(0);

                
            }
        }
    }

    /*Getters*/
    public int GetValue()
    {
        return _value;
    }

    public int GetLevel(){
        return _level;
    }

    /*Fonction qui permet de switcher le mode de la tour en fonction d'un entier (0 = normale, 1 = tour de feu, 2 = tour de zone)*/
    /*Modifie les booléen de la classe*/
    public void SwitchMode(int i){
        Debug.Log("on switch");
        Color actualColor = Color.white;
        switch (i) {
            case 0:
                isFireTower = false;
                isAOETower = false;
                break;
            case 1:
                isFireTower = true;
                isAOETower = false;
                actualColor = Color.red;
                break;
            case 2:
                isFireTower = false;
                isAOETower = true;
                actualColor = Color.green;
                break;
        }
        GetComponent<Renderer>().material.SetColor("_Color",actualColor);
    }

    /*Renvoie un entier entre 0 et 2 qui représente l'état de la tour*/
    public int State(){
        if (isFireTower) {return 1;} else if (isAOETower) {return 2;} else {return 0;};
    }

    /*Fonction de changement de volume du tir des tours*/
    public static void changeVolumeTower(float value){
        _volume = value <= 1 ? value : 1;
    }

}
