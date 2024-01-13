using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Bowl
{
    public Item item;
    public int amount;
}


[RequireComponent(typeof(CraftingAPI))]
public class CookingSystem : MonoBehaviour
{
    public List<Item> materials;
    public Bowl bowl;

    public List<FoodRecipe> recipes;
    public FoodRecipe outputrecipe;
    public FoodRecipe basic;

    public List<Image> matIcon;

    CraftingAPI api;

    bool cooking;
    bool prepared;

    // Start is called before the first frame update
    void Awake()
    {
        api = GetComponent<CraftingAPI>();

    }

    public void OnCall()
    {
        for(int i = 0; i < matIcon.Count; i++)
        {
            if (materials[i] != null)
                matIcon[i].sprite = materials[i].itemIcon;
        }

        outputrecipe = api.CheckRecipe(recipes, materials);
        if(outputrecipe == null)
        {
            outputrecipe = basic;
        }


        Reload();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetToCook( Item item, int index)
    {
        if(item.ID >= 31000)
        {
            materials[index] = (FoodObject)item;
            OnCall();
        }
        
    }

    public IEnumerator Cooking()
    {
        if (!cooking)
        {
            cooking = true;
            int c = 0;
            foreach (var mat in materials)
            {
                c++;
            }


            if (c >= 3)
            {
                yield return new WaitForSeconds(outputrecipe != null ? outputrecipe.time : 1f);

                /*for (int i = 0; i < 5; i++)
                {
                    materials[i] = null;
                }*/
                prepared = true;

            }
        }
        
    }

    public void Reload()
    {
        StopCoroutine(Cooking());
        
        int c = 0;
        foreach (var mat in materials)
        {
            c++;
        }

        if (c < 3)
        {
            StopCoroutine(Cooking());
        }
        else
        {
            StartCoroutine(Cooking());
        }



        if (prepared)
        {
            if(bowl.amount >= 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    materials[i] = outputrecipe != null? outputrecipe.output : null;
                }
            }
        }
    }


    private void OnDestroy()
    {
        StopCoroutine(Cooking());
    }
}
