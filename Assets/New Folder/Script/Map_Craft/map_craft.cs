using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CraftingAPI))]
public class map_craft : MonoBehaviour
{
    CraftingAPI craftingAPI;
    public List<CreateRecipe> recipes;

    public Collider[] colliders;


    public bool canCraft;
    public LayerMask detections;

    public List<IDandAmount> items;

    Dictionary<Item, int> recorded = new Dictionary<Item, int>();
    public int pos;

    void Start()
    {
        craftingAPI = GetComponent<CraftingAPI>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) )
        {
            colliders = new Collider[0];

            recorded.Clear();
            items.Clear();
            pos = 0;

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Collider[] childColliders = Physics.OverlapSphere(gameObject.transform.GetChild(i).position, 1 , detections );
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
        }
    }


    void StartCraft()
    {



    }

}
