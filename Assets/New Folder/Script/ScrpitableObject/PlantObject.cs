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
        type = ItemType.Plant;       //�}�� COMPILE ����w�q�䬰FOOD
        gainingType = forRatioType.normalItem;
    }
}
