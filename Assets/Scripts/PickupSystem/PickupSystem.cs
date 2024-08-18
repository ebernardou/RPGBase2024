using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;

    [SerializeField]
    private DialogueUI dialogueUI;

    private Collider2D thisCollision;
    private bool isInArea;
    private bool itemHasBeenAdded;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        thisCollision = collision;
        if (collision.gameObject.GetComponent<Item>() != null)
        {
            /*if (collision.gameObject.activeInHierarchy)
            {
                Debug.Log(collision.gameObject.name);*/
                isInArea = true;
                itemHasBeenAdded = false;
                /*Debug.Log("in area");
            }*/
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {            
            AddItemToInventory(thisCollision);
            //isInArea = false;
        }
}

    private void AddItemToInventory(Collider2D collision)
    {
        if (!itemHasBeenAdded)
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                int remainder = inventoryData.AddItem(item.aInventoryItem, item.aQuantity);
                if (remainder == 0)
                {
                    item.DestroyItem();
                }
                else
                    item.aQuantity = remainder;
                itemHasBeenAdded = true;
            }
        }
    }
}
