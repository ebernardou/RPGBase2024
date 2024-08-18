using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryDescription : MonoBehaviour
{

    [SerializeField]
    private TMP_Text itemName;
    [SerializeField]
    private TMP_Text itemDescription;

    public void Awake()
    {
        ResetDescription();
    }

    public void ResetDescription()
    {
        this.itemName.text = "";
        this.itemDescription.text = "";
    }

    public void SetDescription(string itemName, string itemDescription)
    {
        this.itemName.text = itemName;
        this.itemDescription.text = itemDescription;
    }




}
