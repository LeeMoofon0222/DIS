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

    public Item toSmelt;

    public bool issmelting;

    public AudioSource smeltingSound;

    [SerializeField] float smeltingTime;

    int i;

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
