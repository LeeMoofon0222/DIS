using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public GameObject detectBox;
    SphereCollider checkArea;

    public Transform uishow;
    bool showed;
    
    public List<Vector2> itemIDs;
    public List<CreateRecipe> recipeChoose;
    bool itemCheck1 , itemCheck2;

    float _mW;

    public List<CreateRecipe> recipes;
    
    int rcn;

    public LayerMask itemMask;

    public Transform uiContrainer;

    bool regenerateUI;


    public ParticleSystem anvilCraft;


    //public MonoScript script;

    void Awake()
    {
        checkArea = detectBox.GetComponent<SphereCollider>();
    }


    void Update()
    {
        Collider[] colliderDetected = Physics.OverlapSphere(detectBox.transform.position, checkArea.radius / 2, itemMask);

        itemIDs.Clear();
        
        foreach(Collider collider in colliderDetected)
        {
            if(collider.gameObject.TryGetComponent(out ItemObject IO))
            {
                
                
                itemIDs.Add(new Vector2(IO.item.ID, 1));
            }
        }

        detectRepeat();

        rcn = ChangeRecipe();

        while(itemIDs.Count < 3)
        {
            itemIDs.Add(new Vector2(0,0));
        }
        n_CheckRecipe();

        if(Input.GetAxis("Mouse ScrollWheel") != 0 || regenerateUI)
        {
            showed = false;
            regenerateUI= false;
            if (uishow.childCount != 0)      //²M°£
            {
                foreach (Transform child in uishow)
                {
                    Destroy(child.gameObject);
                }
            }

        }
        
        if (!showed)
        {
            if(recipeChoose.Count != 0)
                Instantiate(recipeChoose[rcn].craftresult, uishow);
                showed = true;
        }
        
        if(Input.GetKeyDown(KeyCode.Return) && recipeChoose.Count != 0)
        {
            anvilCraft.Play();
            foreach (Collider collider in colliderDetected)
            {
                if (collider.gameObject.TryGetComponent(out ItemObject IO))
                {
                
                    Destroy(IO.gameObject);
                }
            }
            
            foreach (Transform child in uiContrainer)
            {
                Destroy(child.gameObject);
            }

            GameObject result = Instantiate(recipeChoose[rcn].output, anvilCraft.transform.position, Quaternion.identity);
            result.GetComponent<ItemObject>().SceneSpawned = true;
            itemIDs.Clear();
            recipeChoose.Clear();
        }
    }


    void n_CheckRecipe()
    {
        recipeChoose.Clear();
        for (int i = 0; i < recipes.Count; i++)
        {
            /*itemCheck1 = false;
            itemCheck2 = false;

            if((itemIDs.Exists(itm => itm.x == recipes[i].materials[0].ID) && itemIDs[0].y >= recipes[i].materials[0].amount) || recipes[i].materials[0].ID == 0)
            {
                itemCheck1 = true;
            }
            else
            {
                itemCheck1 = false;
            }
            if((itemIDs.Exists(itm => itm.x == recipes[i].materials[1].ID) && itemIDs[1].y >= recipes[i].materials[1].amount) || recipes[i].materials[1].ID == 0)
            {
                itemCheck2 = true;
            }
            else
            {
                itemCheck2 = false;
            }
            */

            int itemcheck = 0;
            foreach (var material in recipes[i].materials)
            {
                if(material.ID != 0)
                {
                    if (itemIDs.Count != 0)
                    {
                        foreach (var a_item in itemIDs)
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
                if (!recipeChoose.Exists(type => type == recipes[i]))
                {
                    recipeChoose.Add(recipes[i]);
                }
            }
            else
            {
                regenerateUI = true;
            }


            /*if (itemCheck1 == true && itemCheck2 == true)
            {
                if (!recipeChoose.Exists(type => type == recipes[i]))
                {
                    recipeChoose.Add(recipes[i]);
                }

            }
            else
            {
                itemCheck1 = false;
                itemCheck2 = false;
                recipeChoose.Clear();
            }*/

        }
    }


    void detectRepeat()
    {
        for (int i = 0; i < itemIDs.Count; i++)
        {
            for(int j = 0; j < itemIDs.Count; j++)
            {
                if((itemIDs[i].x == itemIDs[j].x) && i != j)
                {
                    int _amt = Mathf.RoundToInt(itemIDs[i].y);
                    _amt++;
                    itemIDs[i] = new Vector2(itemIDs[i].x , _amt);

                    itemIDs[j] = new Vector2(0, 0);
                }
            }
        }
    }

    public int ChangeRecipe()
    {
        float mW = Input.GetAxis("Mouse ScrollWheel") * 10;
        _mW += mW;

        if(recipeChoose.Count >= 2)
        {
            _mW = Mathf.Repeat(_mW, recipeChoose.Count);
        }
        else
        {
            _mW = 0;
        }
       
        int i_mW = Mathf.RoundToInt(_mW);
        return i_mW;
    }
}
