using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType      //namespace 底下所含的全域變數
{
    Food,
    Tool,
    Loot,
    Plant,
    NPC,
    Dig
}
public enum forRatioType
{
    PickAxe,
    Axe,
    Sword,
    normalItem
}

public abstract class Item : ScriptableObject
{
    public int ID;

    public int itemHealth;
    public bool stackable;

    public GameObject itemObject;
    public GameObject spawntoscene;
    public GameObject UIObject;
    

    public string itemName;
    public int itemWeights;

    public Sprite itemIcon;
    

    public ItemType type;
    
    public forRatioType gainingType;

    public string desciption;

    [Header("Place")]
    public bool canThrow;
    public bool placeAble;
    public GameObject Preplace;

    



    

}
