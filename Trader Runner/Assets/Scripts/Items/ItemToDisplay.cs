using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemToDisplay : MonoBehaviour
{
    [SerializeField] private GameObject defaultItem;

    private void Start()
    {
        Destroy(defaultItem);
        defaultItem= Instantiate(GameManager.Instance.GetItemGameObject(), transform);
        defaultItem.transform.localPosition=Vector3.zero;
    }
}
