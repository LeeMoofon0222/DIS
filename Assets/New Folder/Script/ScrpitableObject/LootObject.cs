using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Looting Object", menuName = "Inventory System/Items/LootingObject")]
public class LootObject : Item
{


    public string Drop;
    public void Awake()
    {
        type = ItemType.Loot;
        gainingType = forRatioType.normalItem;
    }

}
