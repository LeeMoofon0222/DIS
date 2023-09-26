using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/FoodObject")]
public class FoodObject : Item
{
    public float healing;
    public float satiety;

    public bool canbeBBQ;
    

    public List<Material> roastStep;
    public List<float> stepTime;        //由末到初

    public List<FoodObject> foodWithin;
    

    public void Awake()
    {
        type = ItemType.Food;       //腳本 COMPILE 之初定義其為FOOD
        gainingType = forRatioType.normalItem;
    }
}
