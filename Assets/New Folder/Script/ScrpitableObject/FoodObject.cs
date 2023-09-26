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
    public List<float> stepTime;        //�ѥ����

    public List<FoodObject> foodWithin;
    

    public void Awake()
    {
        type = ItemType.Food;       //�}�� COMPILE ����w�q�䬰FOOD
        gainingType = forRatioType.normalItem;
    }
}
