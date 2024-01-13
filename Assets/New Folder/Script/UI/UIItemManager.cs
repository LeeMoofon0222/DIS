using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEditor.Progress;


public class UIItemManager : MonoBehaviour
{
    public Item item;
    public int pNum;        //0322

    public int amount;
    public int doneness;
    public int health;

    public int pos;

    public AudioClip select;
    public AudioSource source;

    [Header("For SlotSystem")]
    GameObject player;
    public bool generated;
    public int slotnum;
    public Sprite noneSprite;
    bool fromslot;
    PlayerInventoryController PIC;

    public void OnClick()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.TryGetComponent(out PlayerInventoryController PIC))
        {
            PIC.storagemode= 0;
            PIC.StoreMainhandItem(item , pNum);     //0322
            PIC.IdiotInformationManger();

            PIC.uiSources.PlayOneShot(PIC.selectClip);

        }
    }

    public void getPos()        //存東西
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.TryGetComponent(out PlayerInventoryController PIC))
        {
            PIC.StoreItem(pos);

            PIC.uiSources.PlayOneShot(PIC.selectClip);

            PIC.UpdateDisplay();
        }
    }

    public void StorageOnClick()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.TryGetComponent(out PlayerInventoryController PIC))
        {
            if(PIC.pre_ItemSlot == null)
            {
                PIC.SaveSelected(item, amount, doneness, health, pos);

                PIC.storagemode = 1;

                PIC.uiSources.PlayOneShot(PIC.selectClip);
            }
            PIC.UpdateDisplay();
        }
        
    }

    public void SetItem()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.TryGetComponent(out PlayerInventoryController PIC))
        {
            if (PIC.pre_ItemSlot == null)
            {
                PIC.SaveSelected(item, amount, doneness, health, pos);

                PIC.storagemode = 1;

                PIC.uiSources.PlayOneShot(PIC.selectClip);
            }
            PIC.UpdateDisplay();
        }


    }

    public void SetItemOnSlot(Item _item , int _amount)     //存入物品到欄位
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.TryGetComponent(out PlayerInventoryController PIC))
        {
            if (PIC.pre_ItemSlot != null )
            {
                item = _item;
                amount = _amount;

                if(_item.ID == 10022)
                {
                    PIC.cookingSystem.bowl.item = _item;
                    PIC.cookingSystem.bowl.amount += 1;
                }
                //PIC.cookingSystem.bowl.item = _item;
                //PIC.cookingSystem.bowl.amount += 5;



                var slot = PIC.bowlSlot;
                if(slot.transform.GetChild(0).name == "SlotImage")
                {
                    slot.transform.GetChild(0).GetComponent<Image>().sprite = _item.itemIcon;
                }

                Debug.Log("Placed");

                
                fromslot = true;

                //PIC.uiSources.PlayOneShot(PIC.selectClip);
            }
            PIC.UpdateDisplay();
        }
    }

    public void ReplaceItemOnSlot()     //取回物品至背包
    {
        if (fromslot)
        {
            fromslot= false;
            return;
        }


        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.TryGetComponent(out PlayerInventoryController PIC))
        {
            if (PIC.pre_ItemSlot == null && item != null)
            {
                PIC.addItemPanel.SetActive(true);
            }


            if (item != null)
            {
                Debug.Log("Test");
                PIC.pre_ItemSlot = item;
                PIC.pre_amount = amount;
                PIC.pre_itemhealth = health;
                PIC.pre_itemdoneness = doneness;
                PIC.slotsys_slotproper = this;

                if(item.ID == 10022)
                {
                    if(PIC.cookingSystem.bowl.amount > 1)
                    {
                        //PIC.cookingSystem.bowl.item = null;
                        PIC.cookingSystem.bowl.amount -= 1;
                    }
                    else
                    {
                        PIC.cookingSystem.bowl.item = null;
                        PIC.cookingSystem.bowl.amount -= 1;
                    }
                    
                }



                item = null;
                amount= 0;
                health= 0;
                doneness= 0;

                //PIC.uiSources.PlayOneShot(PIC.selectClip);
            }
            
            PIC.UpdateDisplay();
        }

    }

    public void RepairUI()
    {
        if (this.transform.GetChild(0).name == "SlotImage")
        {
            this.transform.GetChild(0).GetComponent<Image>().sprite = item != null ? item.itemIcon : noneSprite;
        }
    }

 

    public void OnEnable()
    {
        if (generated)
        {
            
            


            player = GameObject.FindGameObjectWithTag("Player");
            if(player.TryGetComponent(out PlayerInventoryController _PIC))
            {
                PIC = _PIC;
                /*if(PIC.cookingSystem != null)
                {
                    if(slotnum < 5)
                    {
                        item = PIC.cookingSystem.materials[slotnum];
                        amount = PIC.cookingSystem.materials[slotnum] != null ? 1:0;
                        health = 1;
                        doneness = 0;


                    }
                    else
                    {
                        item = PIC.cookingSystem.bowl.item;
                        amount = PIC.cookingSystem.bowl.amount;
                        health = 1;
                        doneness = 0;
                    }
                }*/
                ForCookingSys();









            }

            if (this.transform.GetChild(0).name == "SlotImage")
            {
                this.transform.GetChild(0).GetComponent<Image>().sprite = item != null? item.itemIcon: noneSprite;
            }


        }



    }
    void ForCookingSys()
    {
        if (PIC.cookingSystem != null)
        {
            if (slotnum < 5)
            {
                item = PIC.cookingSystem.materials[slotnum];
                amount = PIC.cookingSystem.materials[slotnum] != null ? 1 : 0;
                health = 1;
                doneness = 0;


            }
            else
            {
                item = PIC.cookingSystem.bowl.item;
                amount = PIC.cookingSystem.bowl.amount;
                health = 1;
                doneness = 0;
            }
        }
        if (this.transform.GetChild(0).name == "SlotImage")
        {
            this.transform.GetChild(0).GetComponent<Image>().sprite = item != null ? item.itemIcon : noneSprite;
        }
    }




}
