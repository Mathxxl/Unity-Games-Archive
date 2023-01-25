using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class mère des objets
/// </summary>
public class Item : MonoBehaviour
{
    #region Enum
    
    public enum ItemType
    {
        Bleu,
        Violet,
        Orange,
        Vert
    };
    
    #endregion

    #region Attributes

    private ItemType type;
    private int value = 1;
    private bool taken = false;
    [SerializeField] private UnityEvent onDisappear;
    [SerializeField] private float timeDisappear;
    
    #endregion

    #region Mono Behaviour handlers

    private void Start()
    {
        switch (GameManager.Instance.biome)
        {
            case 0:
                type = ItemType.Orange;
                break;
            case 1:
                type = ItemType.Vert;
                break;
            case 2:
                type = ItemType.Bleu;
                break;
            case 3:
                type = ItemType.Violet;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.transform.CompareTag("Player") && !taken)
        {
            ItemManager.Instance.GainItem(type, value);
            taken = true;
            onDisappear.Invoke();
            StartCoroutine(Disappear(timeDisappear));
        }
        else
        {
            Debug.Log($"Item {transform.name} is in contact of gameobject : {other.gameObject.name}");
        }
    }

    #endregion

    #region Public Methods

    

    #endregion

    #region Private Methods

    IEnumerator Disappear(float time = 0)
    {
        yield return (time == 0 ? null : new WaitForSeconds(time));
        Destroy(gameObject);
    }

    #endregion
}
