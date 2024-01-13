using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


[CreateAssetMenu(fileName = "Storage", menuName = "Storage System")]
[Serializable]
public class StorageRecord : ScriptableObject
{
    public List<Storage> Storages = new List<Storage>(); 
}

[Serializable]
public class Storage
{

    public List<StorageSlot> Container = new List<StorageSlot>();
    //Container.Capacity = 30;
    int slotindex;
    public bool assigned;

    
    public Storage(int _slotindex)
    {
        slotindex = _slotindex;
        for(int i = 0; i < _slotindex; i++)
        {
            Container.Add(new StorageSlot(null, 0, 0, 0));
        }
        Container.Capacity = _slotindex;
    }


}

[Serializable]
public class StorageSlot
{
    public Item item;
    public int amount;
    public int item_Health;     
    
    //public int pNum;        

    public int doneness;

    public StorageSlot(Item _item, int _amount, int _item_Health, int _doneness /*, int _pNum*/)
    {
        item = _item;
        amount = _amount;
        item_Health = _item_Health;
        doneness = _doneness;
        //pNum = _pNum;
    }
}
 