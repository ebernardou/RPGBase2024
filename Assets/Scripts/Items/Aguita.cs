using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aguita : Item
{
    [field: SerializeField]
    public ItemSO InventoryItem { get; private set; }

    [field: SerializeField]
    public int Quantity { get; set; } = 1;

    public override void Interact(PlayerController player)
    {

    }

    private void Start()
    {
        aInventoryItem = InventoryItem;
        aQuantity = Quantity;
    }
}
