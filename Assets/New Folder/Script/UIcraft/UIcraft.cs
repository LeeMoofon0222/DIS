using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIcraft : MonoBehaviour
{
    public Makecount Makecount;
    public InventoryRecord inventoryRecord; 
    public List<Item> storeItem;

    [SerializeField] List<bool> itemCheck;

    public CreateRecipe recipe;

   
    public GameObject craftButton;

    public PlayerInventoryController PIC;

    void Update()
    {
        
        if(recipe != null)
        {
            CheckRecipe();
        }

    }


    void CheckRecipe()
    {   
        for(int j = 0; j < itemCheck.Count; j++)
        {
            itemCheck[j] = false;
        }

        for(int i = 0; i < 3; i++)
        {
            if (inventoryRecord.FindItem(recipe.materials[i].ID) || recipe.materials[i].ID == 0)
            {
                var itm = inventoryRecord.IDtoItem(recipe.materials[i].ID);

                if (inventoryRecord.GetItemAmount(itm)  >= recipe.materials[i].amount * Makecount.Count)
                {
                    itemCheck[i] = true;
                    foreach (var _item in inventoryRecord.Container)
                    {
                        if (_item.item.ID == recipe.materials[i].ID)
                        {
                            storeItem[i] = _item.item;

                            break;
                        }
                    }
                }
                else
                {
                    itemCheck[i] = false;
                    storeItem[i] = null;
                }
            }
            else
            {
                itemCheck[i] = false;
                storeItem[i] = null;
            }
        }


        if (itemCheck[0] == true && itemCheck[1] == true && itemCheck[2] == true)
        {
            craftButton.SetActive(true);  
        }
        else
        {
            craftButton.SetActive(false);
        }

    }

    public void recipePick(recipeManager _recipe)
    {
        recipe = _recipe.recipe;

    }
    public void CraftItem()
    {   if(recipe != null)
        {
            for (int i = 0; i < 3; i++)
            {
                if (recipe.materials[i].ID != 0)
                {
                    
                    inventoryRecord.DecreesItem(storeItem[i], recipe.materials[i].amount * Makecount.Count, 0);    //0322

                    PIC.UpdateDisplay();

                    if (inventoryRecord.GetItemAmount(storeItem[i]) == recipe.materials[i].amount)
                    {
                        PIC.pnum_extra = i;
                    }
                }
            }
            PIC.pnum_extra = 0;

            inventoryRecord.AddItem(recipe.Itemoutput, recipe.outputAmount * Makecount.Count, recipe.Itemoutput.itemHealth, 0);
            PIC.UpdateDisplay();
            Makecount.Count = 01;
            Makecount.Count_text.text = Makecount.Count.ToString();
            recipe = null;
            StartCoroutine(waitFrame());

        }

    }


    IEnumerator waitFrame()
    {
        yield return new WaitForEndOfFrame();
        //Debug.Log(inventory.Container[i].pNum);
        PIC.UpdateDisplay();


    }
}
