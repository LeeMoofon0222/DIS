using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void getPos()        //¦sªF¦è
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.TryGetComponent(out PlayerInventoryController PIC))
        {
            PIC.StoreItem(pos);

            PIC.uiSources.PlayOneShot(PIC.selectClip);
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

        }
    }

}
