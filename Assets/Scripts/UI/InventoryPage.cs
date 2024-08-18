using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryPage : MonoBehaviour
{
    [SerializeField]
    private InventoryItem itemPrefab;

    [SerializeField]
    private RectTransform contentPanel;

    [SerializeField]
    private InventoryDescription itemDescription;

    private EventSystem eventSystem;

    public List<InventoryItem> itemList= new List<InventoryItem>();

    public bool isInventoryEnabled = false;
    private int inventorySizeGet;

    public GameObject lastSelected;
    private GameObject currentSelected_Recent;

    private void Update()
    {
        GetLastGameObjectSelected();    
    }

    private void Awake()
    {
        Hide();
        itemDescription.ResetDescription();
        eventSystem = FindObjectOfType<EventSystem>().GetComponent<EventSystem>();
    }

    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            InventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            itemList.Add(uiItem);
        }
        inventorySizeGet = inventorySize;
    }

 
    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if (itemList.Count > itemIndex)
        {
            itemList[itemIndex].SetData(itemImage, itemQuantity);
        }
    }


    public void Show()
    {
        gameObject.SetActive(true);
        isInventoryEnabled = true;
        itemDescription.ResetDescription();
        itemList[0].FirstSelect();
    }

    public void Hide()
    {
        isInventoryEnabled = false;
        gameObject.SetActive(false);
    }

    private void GetLastGameObjectSelected()
    {
        if (eventSystem.currentSelectedGameObject != currentSelected_Recent)
        {
            lastSelected = currentSelected_Recent;

            currentSelected_Recent = eventSystem.currentSelectedGameObject;
        }
    }

    internal void UpdateDescription(int itemIndex, string name, string description)
    {
        itemDescription.SetDescription(name, description);
        eventSystem.SetSelectedGameObject(itemList[itemIndex].border);
    }

    internal void ResetAllItems()
    {
        foreach (var item in itemList)
        {
            item.ResetData();
            item.Deselect();
        }
    }

    /*private void DeselectAllItems()
    {
        for (int i = 0; i < inventorySizeGet; i++)
        {
            itemList[i].borderSprite.enabled = false;
        }
    }*/
}
