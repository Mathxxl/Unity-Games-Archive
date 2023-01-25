using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioData;
    private bool isActiveAudio = true;
    public Camera _camera;

    // Update is called once per frame
    void Update()
    {
        /*On utilise le Raycast pour regarder sur quel collider on clique*/
        if (Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {

                if (hit.transform.gameObject.tag == "eventTrigger"){
                    ObjectEvent(hit.transform.gameObject.name);
                } 
            }

            //Debug.DrawLine(transform.position, hit.point, Color.red);  
        }
    }
    
    
    /*Fonction qui gère les différents événements en fonction de où l'on touche l'objet*/
    void ObjectEvent(string objectName){

        bool evt = true;
        switch (objectName){

            /*Si on clique sur la manivelle : on ouvre la boître et on joue de la musique*/
            case "BoxOpener":
                anim = gameObject.GetComponent<Animator>();
                anim.SetTrigger("Open");
                break;

            /*Si on appuie sur le bouton dans la boite : on joue de la musique*/
            case "BoxButton":
                if (isActiveAudio) {
                    audioData = gameObject.GetComponent<AudioSource>();
                    audioData.Play(0);
                    isActiveAudio = false;
                }
                if (PNJManagement.GetCurrentPNJ() == "aristo") {
                    StaticObject.idComment = 1;
                } else {
                    StaticObject.idComment = 0;
                }
                break;

            /*Si on clique sur le cadenas : le livre s'ouvre*/
            case "Cadenas":
                anim = gameObject.GetComponent<Animator>();
                anim.SetTrigger("Open");
                if (PNJManagement.GetCurrentPNJ() == "vieux") {
                    StaticObject.idComment = 1;
                    CreateDialogues.AddIdSentenceSaid(103);
                    PNJManagement.Instance.ChangeSentenceCurrent();
                } else {
                    StaticObject.idComment = 0;
                }
                break;

            /*Si on clique sur la fleur derrière le miroir*/
            case "paquerette":
                StaticObject.idComment = 0;
                break;

            /*Si on clique sur la fermeture de l'ours en peluche, il s'ouvre*/
            case "Fermeture":
                anim = gameObject.GetComponent<Animator>();
                anim.SetTrigger("Activate");
                if (PNJManagement.GetCurrentPNJ() == "primrose") {
                    StaticObject.idComment = 1;
                } else {
                    StaticObject.idComment = 0;
                }                
                break;

            /*Si on clique sur le haut de la montre, les aiguilles tournent*/
            case "activeMontre":
                anim = gameObject.GetComponent<Animator>();
                anim.SetTrigger("Activate");
                if (PNJManagement.GetCurrentPNJ() == "vieux") {
                    StaticObject.idComment = 1;
                } else {
                    StaticObject.idComment = 0;
                }
                break;

            /*Si on clique sur le klaxon, la voiture fait pouet pouet*/
            case "Icosphere":
                audioData = gameObject.GetComponent<AudioSource>();
                audioData.Play(0);
                StaticObject.idComment = 1;
                break;

            /*On remarque qu'il manque une roue à la voiture*/
            case "roue":
                StaticObject.idComment = 0;
                break;

            /*Si on clique sur les rose du cadre, on les remarques*/
            case "Rose":
                if (PNJManagement.GetCurrentPNJ() == "luke") {
                    StaticObject.idComment = 1;
                } else {
                    StaticObject.idComment = 0;
                }
                break;

            /*On peut remarquer que le vase cassé est ... cassé*/
            case "vaseCasseTrigger":
                StaticObject.idComment = 0;
                break;                

            /*default : on ne fait rien*/
            default:
                evt = false;
                break;
        }

        if (evt)
        {
            IsItemSelect.DeclenchEvent();
        }
    }
}
