using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AdaptativeText : MonoBehaviour
{
    [SerializeField] private GameObject _descriptionObject;
    [SerializeField] private GameObject _prixObject;
    [SerializeField] private GameObject _commentaireObject;
    [SerializeField] private GameObject _nameObject;

    private ListTextObject _listObjects;
    //private TextAsset jsonFiles;
    private int idObjectActuel;
    // Start is called before the first frame update
    void Start()
    {
        SetDescription();
        _commentaireObject.GetComponent<UnityEngine.UI.Text>().text = "Vous ne voyez rien de particulier";
    }

    public void SetDescription()
    {
        /*On va chercher les GameObject Text qui vont s'adapter*/
        _descriptionObject = GameObject.Find("DescriptionText");
        _commentaireObject = GameObject.Find("CommentaireText");
        _nameObject = GameObject.Find("NameText");
        _prixObject = GameObject.Find("PrixValeur");


        _listObjects = new ListTextObject();

        
        /*if (NumberDay.GetDay() == 1) {
            var jsonFiles = Resources.Load<TextAsset>("objects_cycle1_tutoriel");
            Debug.Log("on passe par le jour 1");
        } else {
            var jsonFiles = Resources.Load<TextAsset>("objects_cycle2");
            Debug.Log("on passe par le jour 2");
        }*/

        var jsonFiles = Resources.Load<TextAsset>("objects_cycle" + Mathf.Clamp(NumberDay.GetDay(),1,2));

        _listObjects = JsonUtility.FromJson<ListTextObject>(jsonFiles.ToString());

        idObjectActuel = objectNameToId(StaticObject.activeObject.name);
        

        _descriptionObject.GetComponent<UnityEngine.UI.Text>().text = _listObjects.GetDescriptionByID(idObjectActuel);
        _nameObject.GetComponent<UnityEngine.UI.Text>().text = _listObjects.GetNameByID(idObjectActuel);
        _prixObject.GetComponent<UnityEngine.UI.Text>().text = _listObjects.GetPriceByID(idObjectActuel).ToString();
    }
    
    

    // Update is called once per frame
    void Update()
    {
        if (StaticObject.idComment != -1) {
            _commentaireObject.GetComponent<UnityEngine.UI.Text>().text = _listObjects.GetCommentByID(idObjectActuel)[StaticObject.idComment];
        }
    }

    /*Classe de listes de descriptions d'objets pour le parsing*/
    public class ListTextObject{
        public List<TextObervationObject> ListObjects;

        public string GetNameByID(int id)
        {
            foreach (var obj in ListObjects)
            {
                if (obj.idObject == id)
                {
                    return obj.objectName;
                }
            }

        throw new Exception("Pas d'id correspondant");
        }

        public string GetDescriptionByID(int id)
        {
            foreach (var obj in ListObjects)
            {
                if (obj.idObject == id)
                {
                    return obj.description;
                }
            }

        throw new Exception("Pas d'id correspondant");
        }

        public int GetPriceByID(int id)
        {
            foreach (var obj in ListObjects)
            {
                if (obj.idObject == id)
                {
                    return obj.recommandedPrice;
                }
            }

        throw new Exception("Pas d'id correspondant");
        }

        public List<string> GetCommentByID(int id)
        {
            foreach (var obj in ListObjects)
            {
                if (obj.idObject == id)
                {
                    return obj.commentaries;
                }
            }

        throw new Exception("Pas d'id correspondant");
        }
    }

    /*Classe correspondant à un objet avec son nom, sa description, son prix et ses commentaires*/
    [Serializable]
    public class TextObervationObject{
        /*Nom de l'objet*/
        public string objectName;
        /*Id de l'objet*/
        public int idObject;
        /*Description de l'objet*/
        public string description;
        /*Prix de vente suggéré*/
        public int recommandedPrice;
        /*Liste des réactions possible selon l'endroit qui est appuyé sur l'objet*/
        public List<string> commentaries; 
    }

    public int objectNameToId(string name) {
        switch (name){
            case "Livre - Le Noël de Moustache":
                return 3;
                //break;
            case "Boîte à musique":
                return 1;
                //break;
            case "Vase ancien":
                return 1;
                //break;
            case "Vase moderne":
                return 2;
                //break;
            case "Montre à gousset":
                return 2;
                //break;
            case "Ours en peluche":
                return 3;
                //break;
            case "Cadre photo":
                return 4;
                //break;
            case "Petite voiture":
                return 5;
                //break;
            case "Vieux miroir":
                return 6;
                //break;
            default:
                Debug.Log("Pas d'objet");
                return 0;
                //break;
        }
    }
}
