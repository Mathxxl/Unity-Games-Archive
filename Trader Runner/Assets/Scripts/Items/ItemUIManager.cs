using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class ItemUIManager : MonoBehaviour
{
    private Dictionary<Item.ItemType, TextMeshProUGUI> _dictTypeText;
    private GameObject _textHolder;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Start()
    {
        //Attributes setup
        
        _textHolder = gameObject.transform.GetChild(0).gameObject;
        _dictTypeText = new Dictionary<Item.ItemType, TextMeshProUGUI>();
        
        //Remplissage de notre dictionnaire avec les types et les UI
        
        int i = 0;
        foreach (var type in ItemManager.Instance.itemsTypes)
        {
            if (_textHolder.transform.GetChild(i))
            {
                _dictTypeText.Add(type, _textHolder.transform.GetChild(i).GetComponent<TextMeshProUGUI>());
                i++;
            }
            else
            {
                Debug.LogWarning("Trying to add to UI more types than text");
                return;
            }
        }
        UpdateAllUI(0);
    }

    /// <summary>
    /// Mets à jours tous les textes
    /// </summary>
    /// <param name="value"></param>
    private void UpdateAllUI(int value)
    {
        foreach (var type in _dictTypeText.Keys)
        {
            UpdateUI(type, value);
        }
    }

    /// <summary>
    /// Met à jour le texte associé à une certaine valeur
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    public void UpdateUI(Item.ItemType type, int value)
    {
        if (_dictTypeText.ContainsKey(type))
        {
            _dictTypeText[type].text = $"{type} : {value}";
        }
    }


    public void UpdateScore(int value)
    {
        moneyText.text = value + " CAD";
    }

}
