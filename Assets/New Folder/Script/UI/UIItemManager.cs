using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemManager : MonoBehaviour
{
    public Item item;
    public int pNum;        //0322

    public void OnClick()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.TryGetComponent(out PlayerInventoryController PIC))
        {
            PIC.StoreMainhandItem(item , pNum);     //0322
            PIC.IdiotInformationManger();

        }


    }

}
