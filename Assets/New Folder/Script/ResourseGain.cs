
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LootDrop
{
    public int Randomweight;
    public List<Item> loot;

}

[System.Serializable]
public struct FinalDrop
{
    public int amount;
    public Item loot;

}



public class ResourseGain : MonoBehaviour
{
    public Item baseItem;

    public List<LootDrop> lootdrop;
    public List<FinalDrop> finalDrop;

    int RandNum;

    public InventoryRecord m_inventory;

    int testing;

    public forRatioType _forRatioType;

    public int peritemuGet;



    private void Start()
    {
        foreach (var item in lootdrop)
        {
            testing += item.Randomweight;

        }
    }

    public void tryGetExtraResources()
    {   
        if(lootdrop.Count >= 2)
        {
            RandNum = Random.Range(1, 100);

            for (int i = 0; i < lootdrop.Count; i++)
            {
                if (RandNum > lootdrop[i].Randomweight)
                {
                    RandNum -= lootdrop[i].Randomweight;
                }
                else
                {
                    if (lootdrop[i].loot[0] != null)
                    {
                        if (lootdrop[i].loot.Count > 1)
                        {
                            int rand = Random.Range(0, lootdrop[i].loot.Count - 1);
                            m_inventory.AddItem(lootdrop[i].loot[rand], 1, 1, 0);

                        }
                        else
                        {
                            m_inventory.AddItem(lootdrop[i].loot[0], 1, 1, 0);

                        }
                    }

                    break;
                }
            }
        }
    }

    public void GainMoreResources(ToolObject nowHolding)
    {

        if(baseItem != null )m_inventory.AddItem(baseItem, peritemuGet * nowHolding.ratio,1,0);

    }

    public void GetFinalResources()
    {
        if(finalDrop.Count > 0)
        {
            foreach (var item in finalDrop)
            {
                m_inventory.AddItem(item.loot, item.amount,item.loot.itemHealth,0);
            }
        }
    }


}
