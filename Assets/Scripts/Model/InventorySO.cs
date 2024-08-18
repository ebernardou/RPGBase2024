using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu]

public class InventorySO : ScriptableObject
{
    [SerializeField]
    private List<InventoryItemStruct> inventoryItems;

    [field: SerializeField]
    public int Size { get; private set; } = 10;

    //public event Action<Dictionary<int, InventoryItemStruct>> OnInventoryUpdated;

    public void Initialize()
    {
        inventoryItems = new List<InventoryItemStruct>();
        for (int i = 0; i < Size; i++)
        {
            inventoryItems.Add(InventoryItemStruct.GetEmptyItem());
        }
    }

    public int AddItem(ItemSO item, int quantity)
    {
        if (item.IsStackable == false)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                while(quantity > 0 && IsInventoryFull() == false)
                {
                    quantity -= AddItemToFristFreeSlot(item, 1);
                }
                InformAboutChange();
                return quantity;
            }
        }
        quantity = AddStackableItem(item, quantity);
        InformAboutChange();
        return quantity;
    }

    private int AddItemToFristFreeSlot(ItemSO item, int quantity)
    {
        InventoryItemStruct newItem = new InventoryItemStruct
        {
            item = item,
            quantity = quantity,
        };

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].isEmpty)
            {
                inventoryItems[i] = newItem;
                return quantity;
            }
        }
        return 0;
    }

    private bool IsInventoryFull()
    => inventoryItems.Where(item => item.isEmpty).Any() == false;

    private int AddStackableItem(ItemSO item, int quantity)
    {
        for (int i= 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].isEmpty)
            {
                continue;
            }
            if (inventoryItems[i].item.ID == item.ID)
            {
                int ammountPossibleToTake =
                    inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;

                if (quantity > ammountPossibleToTake)
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStackSize);
                    quantity -= ammountPossibleToTake;
                }
                else
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);
                    InformAboutChange();
                    return 0;
                }
            }
        }
        while (quantity > 0 && IsInventoryFull() == false)
        {
            Debug.Log("pero por qué");
            int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
            quantity -= newQuantity;
            AddItemToFristFreeSlot(item, newQuantity);
        }
        return quantity;
    }

    public Dictionary<int, InventoryItemStruct> GetCurrentInventoryState()
    {
        Dictionary<int, InventoryItemStruct> returnValue =
            new Dictionary<int, InventoryItemStruct>();

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].isEmpty)
            {
                continue;
            }
            returnValue[i] = inventoryItems[i];
        }
        return returnValue;
    }

    public InventoryItemStruct GetItemAt(int itemIndex)
    {
        return inventoryItems[itemIndex];
    }

    private void InformAboutChange()
    {
        InventoryController inventoryController = FindObjectOfType<InventoryController>();
        inventoryController.UpdateInventoryUI(GetCurrentInventoryState());
    }

    public void AddItem(InventoryItemStruct item)
    {
        AddItem(item.item, item.quantity);
    }
}

[Serializable]

public struct InventoryItemStruct
{
    public int quantity;
    public ItemSO item;

    public bool isEmpty => item == null;

    public InventoryItemStruct ChangeQuantity(int newQuantity)
    {
        return new InventoryItemStruct
        {
            item = this.item,
            quantity = newQuantity,
        };
    }

    public static InventoryItemStruct GetEmptyItem()
        => new InventoryItemStruct
        {
            item = null,
            quantity = 0,
        };

}