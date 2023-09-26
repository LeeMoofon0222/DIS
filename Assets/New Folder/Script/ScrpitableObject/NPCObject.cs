using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Tool&Equipment Object", menuName = "Inventory System/Items/NPCObject")]

public class NPCObject : Item
{
    public string NPCType;
    public float Health;
    public float attack;
    

    // Start is called before the first frame update
    void Awake()
    {
        type = ItemType.NPC;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
