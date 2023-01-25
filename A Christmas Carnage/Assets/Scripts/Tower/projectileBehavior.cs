using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileBehavior : MonoBehaviour
{
    [SerializeField] private float _speed = 10.0f;

    [SerializeField] private int _damage;
    private GameObject _focus;
    [SerializeField] private bool isFire;
    [SerializeField] private bool isAOE;


    void Update()
    {
        /*si l'ennemi existe toujours, on se déplace vers lui, sinon on détruit le missile*/
        if (_focus != null) {
            transform.position = Vector3.MoveTowards(transform.position, _focus.transform.position, Time.deltaTime * _speed);
        } else {
            Destroy(gameObject);
        }
    }

/*Si on recontre un collider et que c'est un ennemy, on lui inflige des dégâts en appelant sa fonction de damage avant de détruire le missile*/
    void OnTriggerEnter(Collider entity){
        var _ennemy = entity.gameObject;
        if (_ennemy.CompareTag("ennemy")){
            /*Si c'est un projectile de feu, on ralentit l'ennemi puis on lui inflige moins de dégâts*/
            if (isFire){
                var _speed = _ennemy.GetComponent<FollowPath>().getSpeed();
                _ennemy.GetComponent<FollowPath>().SetSpeed(0.9f*_speed);
                _ennemy.GetComponent<EnemyScript>().ReceiveDamage(_damage-5);
            } else {
                _ennemy.GetComponent<EnemyScript>().ReceiveDamage(_damage);
            }
            Destroy(gameObject,0.01f);
        }
    }

/*Setters*/
    public void setFocus(GameObject cible){
        _focus = cible;
    }

    public void SetSpeed(float s){
        _speed = s;
    }
}
