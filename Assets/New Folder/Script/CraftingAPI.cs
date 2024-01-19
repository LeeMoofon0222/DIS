using System.Collections;
using System.Collections.Generic;
using System.Net.Cache;
using UnityEngine;


[System.Serializable]
public class IDandAmount
{
    public Item item;
    public int amount;
    public int itemID;


    public IDandAmount(Item _item = null , int _amount = -1 , int _itemID = -1 , InventoryRecord ir = null)
    {
        item= _item;
        amount= _amount;
        itemID= _itemID;

        if(_itemID != -1 && ir != null)
        {
           item = ir.IDtoItem(_itemID);
        }
        if(_item != null && ir != null)
        {
            itemID = item.ID;
        }
    }
}



public class CraftingAPI : MonoBehaviour
{
    public List<CreateRecipe> CheckRecipe(List<CreateRecipe> recipe ,List<Vector2> checklist)
    {
        /*===========================================================

           checklist是代偵測，recipe示合成表 ，這邊採用鐵占的合成模式

          ============================================================*/

        List<CreateRecipe> output= new List<CreateRecipe>();

        for (int i = 0; i < recipe.Count; i++)
        {
            int itemcheck = 0;
            foreach (var material in recipe[i].materials)
            {
                if (material.ID != 0)
                {
                    if (checklist.Count != 0)
                    {
                        foreach (var a_item in checklist)
                        {
                            if (a_item.x == material.ID && a_item.y >= material.amount && a_item.x != 0)
                            {
                                itemcheck += 1;

                                break;
                            }
                        }

                        if (itemcheck >= 3)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    itemcheck += 1;
                }
            }


            if (itemcheck == 3)
            {

                output.Add(recipe[i]);
                //return true;
            }
        }
        return output;

    }

    public List<CreateRecipe> CheckRecipe(List<CreateRecipe> recipe, InventoryRecord inventory)
    {
        /*===========================================================

           recipe示合成表 ，這邊是直接偵測背包物品

          ============================================================*/

        List<CreateRecipe> output = new List<CreateRecipe>();

        for (int i = 0; i < recipe.Count; i++)
        {
            int itemcheck = 0;
            foreach (var material in recipe[i].materials)
            {
                if (material.ID != 0)
                {
                    if (inventory.FindItem(material.ID))
                    {
                        if(inventory.GetItemAmount(inventory.IDtoItem(material.ID)) >= material.amount)
                        {
                            itemcheck += 1;
                        }
                    }
                }
                else
                {
                    itemcheck += 1;
                }

                if (itemcheck >= 3)
                {
                    break;
                }
            }


            if (itemcheck == 3)
            {
                output.Add(recipe[i]);
            }
        }
        return output;

    }

    public List<CreateRecipe> CheckRecipe(List<CreateRecipe> recipe, List<IDandAmount> checklist)
    {
        /*===========================================================

           recipe示合成表 ，這邊是偵測待測陣列checklist

          ============================================================*/

        List<CreateRecipe> output = new List<CreateRecipe>();

        for (int i = 0; i < recipe.Count; i++)
        {
            int itemcheck = 0;
            foreach (var material in recipe[i].materials)
            {
                if (material.ID != 0)
                {
                    if (checklist.Count != 0)
                    {
                        foreach (var item in checklist)
                        {
                            if (item.itemID == material.ID && item.amount >= material.amount && item.itemID != -1)
                            {
                                itemcheck += 1;

                                break;
                            }
                        }

                        if (itemcheck >= 3)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    itemcheck += 1;
                }

                if (itemcheck >= 3)
                {
                    break;
                }
            }


            if (itemcheck == 3)
            {
                output.Add(recipe[i]);
                print("GET1");
            }
        }
        print(output.Count);
        return output;
    }

    public List<CreateRecipe> CheckRecipe(List<CreateRecipe> recipe, List<IDandAmount> checklist, int checkpoints)
    {
        /*===========================================================

           recipe示合成表 ，這邊是偵測待測陣列checklist

          ============================================================*/

        List<CreateRecipe> output = new List<CreateRecipe>();

        for (int i = 0; i < recipe.Count; i++)
        {
            int itemcheck = 0;
            foreach (var material in recipe[i].materials)
            {
                if (material.ID != 0)
                {
                    if (checklist.Count != 0)
                    {
                        foreach (var item in checklist)
                        {
                            if (item.itemID == material.ID && item.amount >= material.amount && item.itemID != -1)
                            {
                                itemcheck += 1;

                                break;
                            }
                        }

                        if (itemcheck >= checkpoints)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    itemcheck += 1;
                }

                if (itemcheck >= checkpoints)
                {
                    break;
                }
            }


            if (itemcheck == checkpoints)
            {
                output.Add(recipe[i]);
                //print("GET1");
            }
        }
        print(output.Count);
        return output;
    }



    public FoodRecipe CheckRecipe(List<FoodRecipe> recipes, List<Item> materials)
    {
        /*===========================================================

           recipe示合成表 ，這邊是偵測待測陣列，烹飪專用

          ============================================================*/

        int checker = 0;

        //FoodRecipe basic = GetComponent<CookingSystem>().basic;

        foreach(var recipe in recipes)
        {
            checker = 0;
            for(int i = 0; i < materials.Count; i++)
            {
                if (recipe.materials[i] == materials[i] && recipe.materials[i] == null)
                {
                    checker++;
                    
                }

            }
            if(checker >= 5)
            {
                return recipe;
                
            }
        }

        return null;
    }

}
