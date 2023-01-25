using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class towerMenu : MonoBehaviour
{
    public TMP_Text _textLevel;
    public TMP_Text _textCost;
    public TMP_Text _textState;
    private int _level {get; set;}
    private int _cost = 5;
    public GameObject _menu;
    private bool _open = false;

    void Awake(){
        /*On instantie le texte du menu avec des valeurs de base*/
        _level = gameObject.GetComponent<towerBase>().GetLevel();
        _textLevel.text = "Cette tour est de niveau : " + _level.ToString();
        _textCost.text = "Coût d'amélioration : " + (_level*5).ToString();
        _textState.text = "Cette tour est actuellement une tour normale";
    }

    /*Fonction qui place le menu correctement, face au joueur et à une certaine hauteur juste devant la tour*/
    public void openMenu(){
        var _rot = Camera.main.transform.rotation.eulerAngles;
        var _quat = new Quaternion();
        _quat.eulerAngles = new Vector3(0, _rot.y, _rot.z);
        _menu.transform.rotation = _quat;

        var _pos = Camera.main.transform.position;
        _menu.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z); //Camera.main.transform.position + (Camera.main.transform.forward * 5);
        _menu.transform.position = Vector3.MoveTowards(_menu.transform.position, _pos, 1);
        _menu.SetActive(true);
        _open = true;
    }

    /*Fonction pour fermer le menu, consistant simplement à le désactiver*/
    public void closeMenu(){
         _menu.SetActive(false);
        _open = false;
    }

    /*Fonction qui permet d'améliorer une tour si le joueur possède assez d'argent*/
    /*Met le texte à jour avant d'appeler la fonction d'amélioration interne des tours*/
    public void upgrade(){
        if (GestionArgent.ChangeMoney(-_cost))
        {
            _level = _level + 1;
            _cost = _level * 5;
            _textLevel.text = "Cette tour est de niveau : " + _level.ToString();
            _textCost.text = "Coût d'amélioration : " + (_level*5).ToString();
            _textState.text = "Cette tour est actuellement une tour normale. Le coût de changement est de 10.";
            GetComponent<towerBase>().Upgrade();
        }
    }

    /*Fonction qui permet de détruire une tour si elle possède ce script*/
    public void destroy_tower(){
        GestionArgent.ChangeMoney(_level * 3);
        Destroy(gameObject);
    }

    /*Getter de _open, qui décrit si le menu est ouvert ou non*/
    public bool isOpen(){
        return _open;
    }

    /*Fonction qui change le mode d'une tour en fonction d'un entier i passé en paramètre*/
    /*0 = normal, 1 = tour de feu, 2 = tour d'attaque de zone*/
    /*Le coût est de 10. Cette fonction gère essentiellement le coût et l'actualisation du texte d'état avant d'appeler la fonction interne de la tour.*/
    private void changeTowerMode(int i){
        if (GestionArgent.ChangeMoney(-10)){
            var _script = GetComponent<towerBase>();
            _script.SwitchMode(i);
            String state = "normale";
            int scriptState = _script.State();
            if (scriptState == 1) {state = "de feu";} else if (scriptState == 2) {state = "d'attaque de zone";};
            _textState.text = "Cette tour est actuellement une tour " + state + ". Le coût de changement est de 10.";
        } else {
            Debug.Log("vous êtes trop pauvres :(");
        }

    }


    /*Fonctions génériques pour changer d'état, utilisées sur des boutons*/
    public void changeTowerToFire() {
        if (GetComponent<towerBase>().State() != 1) {changeTowerMode(1);};
    }

    public void changeTowerToAOE(){
        if (GetComponent<towerBase>().State() != 2) {changeTowerMode(2);};
    }

    public void changeTowerToNormal(){
        if (GetComponent<towerBase>().State() != 0) {changeTowerMode(0);};
    }


}
