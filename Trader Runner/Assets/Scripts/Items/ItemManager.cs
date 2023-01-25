using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Singleton gérant les objets du joueur, c'est à dire son argent
/// </summary>
public class ItemManager : Singleton<ItemManager>
{
    #region Attributes

    [SerializeField] private ItemUIManager uiManager;
    
    /// <summary>
    /// Les objets obtenus par le joueur
    /// <param name="Item.ItemType">Le type de l'objet</param>
    /// <param name="int">La quantité possédée</param>
    /// </summary>
    private Dictionary<Item.ItemType, int> _items;

    private int _money;
    public int Money
    {
        get => _money;
        set
        {
            if (value < 0)
                return;
            _money = value;
            uiManager.UpdateScore(_money);
        }
    }

    /// <summary>
    /// List of our types
    /// </summary>
    private List<Item.ItemType> _itemsType;

    //Getters of types
    public List<Item.ItemType> itemsTypes => _itemsType;
    
    public override bool UseDontDestroyOnLoad => true;

    #endregion

    #region Mono Behaviour

    protected override void OnAwake()
    {
        //Setup types
        _items = new Dictionary<Item.ItemType, int>();
        _itemsType = new List<Item.ItemType>();
        foreach (Item.ItemType type in Enum.GetValues(typeof(Item.ItemType)))
        {
            _items.Add(type, 0);
            _itemsType.Add(type);
        }
    }

    #endregion
    
    #region Public Methods

    /// <summary>
    /// Getter d'item permettant de récupérer la quantité possédée d'un objet
    /// </summary>
    /// <param name="type">Le type dont on souhaite récupérer la quantité</param>
    /// <returns></returns>
    public int GetItemValue(Item.ItemType type)
    {
        if (_items.ContainsKey(type))
        {
            return _items[type];
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// Fonction permettant d'ajouter des ressources au joueur
    /// </summary>
    /// <param name="type">Le type d'objet</param>
    /// <param name="quantity">La quantité dont on augmente le joueur. NOTE : si la quantité est négative elle sera automatiquement passée en positif.</param>
    public void GainItem(Item.ItemType type, int quantity)
    {
        if (quantity < 0)
        {
            Debug.LogWarning($"Gain value of item {type} is negative; value has been set to positive");
            quantity = -quantity;
        }
        
        if (_items.ContainsKey(type))
        {
            //Update Value
            _items[type] += quantity;
            //Update UI
            uiManager.UpdateUI(type, _items[type]);
        }
        else
        {
           _items.Add(type, quantity);
           _itemsType.Add(type);
        }
    }

    /// <summary>
    /// Fonction permettant de payer une certaine quantité d'une ressource
    /// </summary>
    /// <param name="type">Type de l'objet</param>
    /// <param name="quantity">Quantité payée. NOTE : si la quantité est négative elle sera automatiquement passée en positif.</param>
    public void PayItem(Item.ItemType type, int quantity)
    {
        if (quantity < 0)
        {
            Debug.LogWarning($"Paid value of item {type} is negative; value has been set to positive");
            quantity = -quantity;
        }
        
        if (_items.ContainsKey(type))
        {
            //Check for negative values
            int itemValue = _items[type];
            if (itemValue - quantity < 0)
            {
                Debug.LogWarning($"Can't have a negative value, trying to remove {quantity} from {itemValue}");
                return;
            }
            
            //Update value
            _items[type] -= quantity;
            
            //Update UI
            uiManager.UpdateUI(type, _items[type]);
        }
        else
        {
            Debug.LogWarning($"Trying to pay with a non existent currency : {type}");
        }
    }
    
    #endregion

    #region Private Methods

    

    #endregion
}
