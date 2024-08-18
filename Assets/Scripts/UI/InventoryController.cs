using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    public InventoryPage inventoryUI;

    [SerializeField]
    private InventorySO inventoryData;

    [SerializeField]
    private EventSystem eventSystem;

    public List<InventoryItemStruct> initialItems = new List<InventoryItemStruct>();

    private GameObject currentSelected_Recent;
    private GameObject lastSelected;
    private InventoryItem currentSelectedItem;
    private bool isCheckingSelection;

    private int itemIndex;


    private void Start()
    {
        PrepareUI();
        PrepareInventoryData();
    }

    private void PrepareInventoryData()
    {
        inventoryData.Initialize();
        foreach (InventoryItemStruct item in initialItems)
        {
            if (item.isEmpty)
                continue;
            inventoryData.AddItem(item);
        }
        //inventoryData.OnInventoryUpdated += UpdateInventoryUI;
    }

    public void UpdateInventoryUI(Dictionary<int, InventoryItemStruct> inventoryState)
    {
        inventoryUI.ResetAllItems();
        foreach (var item in inventoryState)
        {
            inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
        }

    }

    private void PrepareUI()
    {
        inventoryUI.InitializeInventoryUI(inventoryData.Size);
    }

    public void HandleDescriptionRequest(int itemIndex)
    {
        InventoryItemStruct inventoryItem = inventoryData.GetItemAt(itemIndex);
        ItemSO item = inventoryItem.item;
        if (inventoryItem.isEmpty)
            inventoryUI.UpdateDescription(itemIndex, "", "");
        else
            inventoryUI.UpdateDescription(itemIndex, item.name, item.Description);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI.isInventoryEnabled == false)
            {
                inventoryUI.Show();
                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    if (item.Value.quantity != 0)
                    {
                        inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
                    }
                }
            }
            else
            {
                inventoryUI.Hide();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inventoryUI.isInventoryEnabled == true)
            {
                inventoryUI.Hide();
            }
        }

        if (inventoryUI.isInventoryEnabled == true)
        {
            currentSelectedItem = eventSystem.currentSelectedGameObject.GetComponentInParent<InventoryItem>();
            itemIndex = GetItemIndex();
            if (lastSelected != eventSystem.currentSelectedGameObject)
            {
                HandleDescriptionRequest(itemIndex);
            }
        }


    }


    private void GetLastGameObjectSelected()
    {
        if (eventSystem.currentSelectedGameObject != currentSelected_Recent)
        {

            lastSelected = currentSelected_Recent;

            currentSelected_Recent = eventSystem.currentSelectedGameObject;
        }
    }
    private int GetItemIndex()
    {
        int index = inventoryUI.itemList.IndexOf(currentSelectedItem);
        return index;
    }
    
}
