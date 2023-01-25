using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerCreator : MonoBehaviour
{
    public GameObject _tower;

    private static towerMenu _MenuOpen;
    
    [SerializeField] private Transform folderTower;
    
    void Update(){
        if (Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            int _layer = 1 << 3;
            

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layer)) {

                if (hit.transform.gameObject.tag == "towerZone"){
                    if (GestionArgent.ChangeMoney(-_tower.GetComponent<towerBase>().GetValue()))
                    {
                        var pos = hit.collider.bounds.center;
                        pos.y += 0.5f;
                        Instantiate(_tower, pos, hit.transform.rotation, folderTower);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int _layer2 = 1 << 9;

            if (_MenuOpen != null)
            {
                _MenuOpen.closeMenu();
                _MenuOpen = null;
            }
            
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layer2)) {
                    //Debug.Log("Menu de la tour : " + hit.transform.gameObject.name);
                    var _menuscript = hit.transform.gameObject.GetComponent<towerMenu>();
                    if (!_menuscript.isOpen()){
                        _menuscript.openMenu();
                        _MenuOpen = _menuscript;
                    } else {
                        _menuscript.closeMenu();
                    }
                }
        }
    }
}
