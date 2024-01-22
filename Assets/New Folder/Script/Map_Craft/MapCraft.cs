using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CraftingAPI))]
public class MapCraft : MonoBehaviour
{
    CraftingAPI craftingAPI;
    public List<CreateRecipe> recipes;
    public List<CreateRecipe> output;

    public Collider[] colliders;
    bool reload;


    //public bool canCraft;
    //public bool regenerate_item;
    public LayerMask detections;

    public List<IDandAmount> items;
    public List<Transform> floating_pos;

    Dictionary<Item, int> recorded = new Dictionary<Item, int>();
    int pos;

    void Start()
    {
        craftingAPI = GetComponent<CraftingAPI>();
    }


    void FixedUpdate()
    {
        if (/*Input.GetKeyDown(KeyCode.U)*/ reload)
        {
            print("reload");

            reload = false;
            colliders = new Collider[0];

            recorded.Clear();
            items.Clear();
            pos = 0;

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i);
                SphereCollider col = child.GetComponent<SphereCollider>();

                Collider[] childColliders = Physics.OverlapSphere(child.position,col.radius , detections );
                colliders = colliders.Concat(childColliders).ToArray();

                
            }

            for (int i = 0; i < colliders.Length; i++)
            {
                //print(colliders[i].gameObject.name);
                //colliders[i].enabled = false;
                //colliders[i].GetComponent<Rigidbody>().isKinematic = true;
                //recorded.Clear();
                //pos = 0;



                Item item = colliders[i].GetComponent<ItemObject>().item;
                IDandAmount sets = new IDandAmount(item, 1, -1, null);

                if (items.Count != 0)
                {
                    if (!recorded.ContainsKey(item))
                    {
                        items.Add(sets);
                        recorded.Add(item, pos);
                        pos++;
                    }
                    else
                    {
                        print(recorded[item]);
                        items[recorded[item]].amount += 1;

                    }
                }
                else
                {
                    items.Add(sets);
                    recorded.Add(item, pos);
                    pos++;
                }




            }

            output = craftingAPI.CheckRecipe(recipes , items);

            int s = 0;
            while (colliders.Length != 0)
            {
                if(s < colliders.Length && s < 7)
                {
                    colliders[s].transform.parent = floating_pos[s];
                    colliders[s].transform.localPosition = Vector3.zero;

                    colliders[s].GetComponent<Rigidbody>().isKinematic = true;
                    colliders[s].isTrigger= true;

                    //Destroy(colliders[s]);

                    s++;
                }
                else
                {
                    break;
                }
            }


            if(output.Count != 0)
            {
                StartCraft();
            }



            


        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            reload = true;

        }
        
    }


    void StartCraft()
    {
        for(int i = 0; i < colliders.Length; i++)
        {
            Destroy(colliders[i].gameObject);
        }

        Vector3 generatepoint = transform.position + new Vector3(0, 2, 0);
        print(generatepoint);

        GameObject resualt = Instantiate(output[0].output, generatepoint, Quaternion.identity);
        resualt.layer = 0;
        resualt.GetComponent<ItemObject>().SceneSpawned = true;

        reload = true;


    }

}
