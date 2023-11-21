using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



[System.Serializable]
public struct smeltrecipe
{
    public Item canSmelt;
    public int amount;

    public Item result;
}


public class Smelter : MonoBehaviour
{
    public List<smeltrecipe> recipe;

    public List<CreateRecipe> new_recipe;

    public Item toSmelt;

    public Item[] new_toSmelt = new Item[3];

    public bool issmelting;

    public AudioSource smeltingSound;

    [SerializeField] float smeltingTime;

    int i;

    CraftingAPI crafting;

    public void Awake()
    {
        crafting = GetComponent<CraftingAPI>();
    }


    bool Search(Item _toFind , int _amount)
    {
        foreach(var obj in recipe)
        {    
            if(_toFind == obj.canSmelt && _amount >= obj.amount)
            {
                return true;
            }
            i++;
        }
        i = 0;
        return false;
    }


    public InventoryRecord m_inventory;

    public GameObject SmeltingLight;

    public List<IDandAmount> idandamounts;

    public List<Transform> showpoints;

    IEnumerator Smelting()
    {
        //Debug.Log("start");
        smeltingSound.enabled= true;
        yield return new WaitForSeconds(smeltingTime);
        smeltingSound.enabled = false;
        //Debug.Log("done");

        SmeltingLight.SetActive(false);

        //m_inventory.AddItem(recipe[i].result,1,0,0);
        GameObject output = Instantiate(recipe[i].result.spawntoscene, this.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity) as GameObject;
        output.GetComponent<ItemObject>().SceneSpawned = true;
        i = 0;
        issmelting=false;
        toSmelt = null;
        
    }

    IEnumerator Smelting(CreateRecipe recipe)
    {
        //Debug.Log("start");
        smeltingSound.enabled = true;
        yield return new WaitForSeconds(smeltingTime);
        smeltingSound.enabled = false;
        //Debug.Log("done");

        SmeltingLight.SetActive(false);

        //m_inventory.AddItem(recipe[i].result,1,0,0);
        GameObject output = Instantiate(recipe.output, this.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity) as GameObject;
        output.GetComponent<ItemObject>().SceneSpawned = true;
        i = 0;
        issmelting = false;
        //toSmelt = null;
        for(int i = 0; i < 3; i++)
        {
            new_toSmelt[i] = null;
        }

    }

    public void Smelt(Item _toSmelt, int _amount)
    {
        if (Search(_toSmelt, _amount))
        {
            toSmelt = _toSmelt;
            issmelting = true;
            m_inventory.DecreesItem(toSmelt.ID, 3);

            SmeltingLight.SetActive(true);
            StartCoroutine(Smelting());
        }

    }

    public void setToSmelt(Item _toSmelt)
    {
        for(int i = 0; i < 3; i++)
        {
            if (new_toSmelt[i] == null)
            {
                m_inventory.DecreesItem(_toSmelt.ID, 1);
                new_toSmelt[i] = _toSmelt;
                var show = Instantiate(_toSmelt.spawntoscene, showpoints[i]);
                show.GetComponent<Rigidbody>().isKinematic= true;
                
                if(i < 2)
                {
                    return;
                }
            }
        }

        issmelting = true;

        for (int j = 0; j < new_toSmelt.Length; j++)
        {
            
            idandamounts[j] = new IDandAmount(new_toSmelt[j],1,-1,m_inventory);
        }

        var output = crafting.CheckRecipe(new_recipe, idandamounts);

        if (output.Count != 0) print(output[0]);
        if (output.Count != 0)
        {
            SmeltingLight.SetActive(true);
            StartCoroutine(Smelting(output[0]));

            foreach(var parent in showpoints)
            {
                Destroy(parent.GetChild(0).gameObject);
            }

        }
        else
        {
            issmelting= false;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        if (issmelting)
        {
            for(int j = 0; j < 3; j++)
            {
                m_inventory.AddItem(toSmelt, 1, 0, 0);
            }
        }
    }
}
