using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CraftingAPI))]
public class map_craft : MonoBehaviour
{
    CraftingAPI craftingAPI;
    Collider[] colliders; 
    void Start()
    {
        craftingAPI = GetComponent<CraftingAPI>();
    }

    // Update is called once per frame
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            colliders = new Collider[0];

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Collider[] childColliders = Physics.OverlapSphere(gameObject.transform.GetChild(i).position, 1);
                colliders = colliders.Concat(childColliders).ToArray();
            }

            for (int i = 0; i < colliders.Length; i++)
            {
                print(colliders[i].gameObject.name);
            }
        }
    }

}
