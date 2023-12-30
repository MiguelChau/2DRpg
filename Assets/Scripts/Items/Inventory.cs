using System.Collections.Generic;
using UnityEngine;

//este script vai ser um singleton
public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<ItemData> startingItems;


    public List<InventoryItem> equipement;
    public Dictionary<ItemDataEquipement, InventoryItem> equipementDict;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDict; //� uma lista que tem 2 values - item data e inventory item

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDict;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;

    [Header("Items Cooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;
    private float flaskCooldown;
    private float armorCooldown;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDict = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDict = new Dictionary<ItemData, InventoryItem>();

        equipement = new List<InventoryItem>();
        equipementDict = new Dictionary<ItemDataEquipement, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        AddStartingItems();

    }

    private void AddStartingItems()
    {
        for (int i = 0; i < startingItems.Count; i++)
        {
            AddItem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemDataEquipement newEquipment = _item as ItemDataEquipement;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemDataEquipement oldEquipment = null;

        foreach (KeyValuePair<ItemDataEquipement, InventoryItem> item in equipementDict)
        {
            if (item.Key.equipementType == newEquipment.equipementType)
                oldEquipment = item.Key;
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipement.Add(newItem);
        equipementDict.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        RemoveItem(_item);

        UpdateUISlot();
    }

    public void UnequipItem(ItemDataEquipement itemToRemove)
    {
        if (equipementDict.TryGetValue(itemToRemove, out InventoryItem value))
        {
            //AddItem(itemToRemove); //esta � a forma que � para retirar o equipamento e voltar para a bag 
            equipement.Remove(value);
            equipementDict.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    private void UpdateUISlot()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemDataEquipement, InventoryItem> item in equipementDict)
            {
                if (item.Key.equipementType == equipmentSlot[i].slotType)
                    equipmentSlot[i].UpdateSlot(item.Value);
            }
        }

        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }



        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipement)
        {
            AddToInventory(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }

        UpdateUISlot();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDict.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDict.Add(_item, newItem);
        }
    }
    private void AddToInventory(ItemData _item)
    {
        if (inventoryDict.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDict.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDict.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDict.Remove(_item);
            }
            else
                value.RemoveStack();
        }

        if (stashDict.TryGetValue(_item, out InventoryItem stashvalue))
        {
            if (stashvalue.stackSize <= 1)
            {
                stash.Remove(stashvalue);
                stashDict.Remove(_item);
            }
            else
                stashvalue.RemoveStack();
        }

        UpdateUISlot();
    }

    public bool CanCraft(ItemDataEquipement _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            if (stashDict.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("Not enough material");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("Not enough material");
                return false;
            }
        }

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }

        AddItem(_itemToCraft);
        Debug.Log("Here is your item" + _itemToCraft.name);

        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipement;
    public List<InventoryItem> GetStashList() => stash;

    public ItemDataEquipement GetEquipment(EquipementType _type) //agora podemos acessar a qualquer item do inventory pelo Type
    {
        ItemDataEquipement equippedItem = null;

        foreach (KeyValuePair<ItemDataEquipement, InventoryItem> item in equipementDict)
        {
            if (item.Key.equipementType == _type)
                equippedItem = item.Key;
        }

        return equippedItem;
    }

    public void UseFlask()
    {
        ItemDataEquipement currentFlask = GetEquipment(EquipementType.Flask);

        if (currentFlask == null)
            return;


        bool canUsedFlask = Time.time > lastTimeUsedFlask + flaskCooldown;

        if (canUsedFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
            Debug.Log("flask in CD");
    }

    public bool UseArmor()
    {
        ItemDataEquipement currentArmor = GetEquipment(EquipementType.Armor);

        if(Time.time > lastTimeUsedArmor + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }

        Debug.Log("CD Armor");
        return false;
    }
}
