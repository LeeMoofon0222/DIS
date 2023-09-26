using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Tool&Equipment Object", menuName = "Inventory System/Items/ToolObject")]
public class ToolObject : Item
{
    
    public float ToolATK;
    public float ToolDEF;
    
    public enum toolType
    {
        weapon,
        axe,
        pickaxe

    }

    public int ratio;



    public void Awake()
    {
        type = ItemType.Tool;
    }
}
