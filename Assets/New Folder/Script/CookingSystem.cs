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

    List<Image> _matIcon;

    CraftingAPI api;

    bool cooking;
    bool prepared;

    public bool canCook;

    public BoxCollider area;
    public LayerMask mask;
    BoxCollider save;

    // Start is called before the first frame update
    void Awake()
    {
        api = GetComponent<CraftingAPI>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.TryGetComponent(out PlayerInventoryController PIC))
        {
            _matIcon = PIC.matIcon;

        }


        // LayerMask mask ;
        Collider[] collides = Physics.OverlapBox(this.transform.position, area.size / 2, Quaternion.identity, mask);
        if(collides.Length != 0)
        {
            canCook = true;
            foreach (var col in collides)
            {
                if (col.isTrigger)
                {
                    col.enabled = false;
                    save = (BoxCollider)col;
                }
                
            }
        }
       
    }

    public void OnCall()
    {
        for(int i = 0; i < _matIcon.Count; i++)
        {
            if (materials[i] != null)
                _matIcon[i].sprite = materials[i].itemIcon;
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
        if(item.ID <= 31000 || item.ID == 31005)
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
                if(mat != null)
                {
                    c++;
                }
                
            }

            
            if (c >= 2)
            {
                print("cooking");
                yield return new WaitForSeconds(outputrecipe != null ? outputrecipe.time : basic.time);

                /*for (int i = 0; i < 5; i++)
                {
                    materials[i] = null;
                }*/
                
                prepared = true;
                Ready();

            }
        }
        
    }
    public void Ready()
    {
        if (prepared)
        {
            print("prepared");
            if (bowl.amount >= 1)
            {
                for (int i = 0; i < 5; i++)
                {
                    materials[i] = outputrecipe != null ? outputrecipe.output : null;

                    if (materials[i] != null)
                        _matIcon[i].sprite = materials[i].itemIcon;

                    repairUI();


                    prepared = false;

                }
            }
        }
        
    }



    public void Reload()
    {
        
        StopCoroutine(Cooking());
        
        int c = 0;
        foreach (var mat in materials)
        {
            if (mat != null)
            {
                c++;
            }
            //c++;
        }

        if (c < 3)
        {
            StopCoroutine(Cooking());
        }
        else
        {
            StartCoroutine(Cooking());
        }

        if(c == 0)
        {
            cooking = false;
            prepared = false;



        }

        Ready();


        
    }

    void repairUI()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.TryGetComponent(out PlayerInventoryController PIC))
        {
            for(int i = 0;  i<PIC.cooking_uislot.Length; i++)
            {
                PIC.cooking_uislot[i].item = materials[i];
                PIC.cooking_uislot[i].RepairUI();
            }
            



        }
        

    }


    private void OnDestroy()
    {
        StopCoroutine(Cooking());
        if(save != null ) save.enabled = true;
    }
}
