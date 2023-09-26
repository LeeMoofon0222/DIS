using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Plant Object", menuName = "Inventory System/Items/PlantObject")]
public class PlantObject : Item
{
    public int period;

    public List<GameObject> plantstep;

    public void Awake()
    {
        type = ItemType.Plant;       //腳本 COMPILE 之初定義其為FOOD
        gainingType = forRatioType.normalItem;
    }
}
