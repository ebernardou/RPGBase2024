using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{

    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private TMP_Text itemQuantity;

    [SerializeField]
    public Image borderSprite;

    [SerializeField]
    public GameObject border;

    private EventSystem eventSystem;

    private bool empty = true;


    public void Awake()
    {
        ResetData();
        //Deselect();
        eventSystem = FindObjectOfType<EventSystem>().GetComponent<EventSystem>();
    }

    public void ResetData()
    {
        this.itemImage.gameObject.SetActive(false);
        empty = true;
    }

    public void Deselect()
    {
        eventSystem.SetSelectedGameObject(null);
    }

    public void SetData(Sprite sprite, int quantity)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.itemQuantity.text = quantity + "";
        empty = false;
    }

    public void FirstSelect()
    {
        eventSystem.SetSelectedGameObject(border);
        border.GetComponent<Button>().Select();
    }


}
