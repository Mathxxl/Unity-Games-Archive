using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitObservableObject : MonoBehaviour
{
    private static GameObject initGameObject;
    private Vector3 objectPosition = new Vector3(0,0,80);
    [SerializeField] private Camera _camera;

    /*On instantie le gameObject contenu dans la variable static associée*/
    void Awake(){
    }

    public void SetNewObject()
    {
        initGameObject = StaticObject.activeObject;
        GameObject go = Instantiate(initGameObject, objectPosition, _camera.transform.rotation);
        go.transform.parent = transform;
        go.GetComponent<ClickableObject>()._camera = _camera;
    }

    public void CloseObject()
    {
        Destroy(transform.GetChild(0).gameObject);

        initGameObject = null;
    }
    
}




public class StaticObject
{
    public static GameObject activeObject;
    public static int idComment = -1;
}
