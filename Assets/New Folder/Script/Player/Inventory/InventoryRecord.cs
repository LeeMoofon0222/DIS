using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Inventory",menuName ="Inventory System To Record/Inventory")]
public class InventoryRecord : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();
    public int weight;

    public bool FindItem(Item item)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == item)
            {
                return true;
            }
        }
        return false;
    }
    public bool FindItem(Item item , int _amount)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == item && Container[i].amount >= _amount)
            {
                return true;
            }
        }
        return false;
    }
    public bool FindItem(int itemID)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item.ID == itemID)
            {
                return true;
            }
        }
        return false;
    }
    public bool FindItem(int itemID , int _amount)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item.ID == itemID && Container[i].amount >= _amount)
            {
                return true;
            }
        }
        return false;
    }

    public int GetItemAmount(Item _item)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item )
            {
                return Container[i].amount;
            }
        }
        return 0;
    }

    public int GetItemAmount(Item _item, int _Pnum)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item && _Pnum == Container[i].pNum)
            {
                //Debug.Log("CheckPoint");
                //Debug.Log(Container[i].doneness);
                return Container[i].amount;
            }
        }
        return 0;
    }

    public Item IDtoItem(int id)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item.ID == id)
            {
                return Container[i].item;
            }
        }
        return null;
    }

    public void AddItem(Item _item, int _amount,int _health, int _doneness)
    {
        bool hasItem = false;
        for(int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item && _item.stackable)
            {
                Container[i].AddAmount(_amount);
                weight += _item.itemWeights * _amount;
                hasItem = true;
                break;

            }
        }
        if (!hasItem)
        {
            Container.Add(new InventorySlot(_item,_amount,_health ,_doneness,-1));
            weight += _item.itemWeights;
        }
        GameObject SystemINformation = GameObject.Find("SystemInformationText");
        SystemTEXTManger systemtextManger = SystemINformation.GetComponent<SystemTEXTManger>();
        if (_amount > 0)
            systemtextManger.Getin(_item, _amount);
    }


    public void DecreesItem(Item _item, int _amount , int _Pnum)        //0322
    {   


        for(int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item && (_Pnum == Container[i].pNum || _Pnum == 0))       //0322
            {
                if (Container[i].amount > 0)
                {
                    Container[i].decreesAmount(_amount);
                    weight -= _item.itemWeights * _amount;
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        //Debug.Log("Faild Decrease Item");
    }
    public void DecreesItem(int id, int _amount)        //0322
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item.ID == id)       //0322
            {
                if (Container[i].amount > 0)
                {
                    Container[i].decreesAmount(_amount);
                    weight -= Container[i].item.itemWeights * _amount;
                    break;
                }
                else
                {
                    break;
                }
            }
        }
    }


    public void breakingItem(Item _item, int _amount, int _Pnum)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item && (_Pnum == Container[i].pNum || _Pnum == 0))       //0323
            {
                //Debug.Log("CheckPoint");
                if(_item.type == ItemType.Tool || _item.type == ItemType.Food)
                {
                    if (Container[i].item_Health > 0)
                    {
                        Container[i].consumeing(_amount);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    public void SetBBQStep(Item _item, int _value, int _Pnum)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item && (_Pnum == Container[i].pNum))       
            {
                //Debug.Log("CheckPoint");
                Container[i].doneness = _value;
                break;
            }
        }
    }
    public int GetBBQStep(Item _item, int _Pnum)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item && _Pnum == Container[i].pNum)
            {
                //Debug.Log("CheckPoint");
                //Debug.Log(Container[i].doneness);
                return Container[i].doneness;
            }
        }
        return 0;
    }

    public int GetItemhealth(Item _item, int _Pnum)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item && _Pnum == Container[i].pNum)
            {
                //Debug.Log("CheckPoint");
                //Debug.Log(Container[i].doneness);
                return Container[i].item_Health;
            }
        }
        return 0;
    }

    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                if (Container[i].amount <= 0 || (Container[i].item_Health <= 0 && (Container[i].item.type == ItemType.Tool || Container[i].item.type == ItemType.Food)))
                {
                    Container.Remove(Container[i]);
                    weight -= _item.itemWeights;
                    break;
                }

            }
        }

    }
    public int ItemCount(int itemID, int _amount)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item.ID == itemID)
            {
                return Container[i].amount;
            }
        }
        return 0;
    }

    public void ReturnItem(int itemID, int _amount)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item.ID == itemID)
            {
                Container[i].amount = _amount;
            }
        }
    }



}

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int amount;

     public int item_Health;      //0322
    //[HideInInspector] 
    public int pNum;        //位址 需要固定時間整理一次 基本上就是陣列的第幾位 必須在特定情況丟出 0322

    public int doneness;

    public InventorySlot(Item _item, int _amount, int _item_Health , int _doneness, int _pNum)
    {
        item = _item;
        amount = _amount;
        item_Health = _item_Health;
        doneness = _doneness;
        pNum = _pNum;   
      
    }

    public void AddAmount(int value)
    {
         amount += value; 
    }
    public void decreesAmount(int value)
    {
        amount -= value;
    }

    public void consumeing(int value)       //0323
    {
        item_Health -= value;
    }


}
