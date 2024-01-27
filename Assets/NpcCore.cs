using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCore : MonoBehaviour
{
    public GameObject[] npcs;

    public GameObject loot;
    //public GameObject vfx;

    GameObject record;

    // Update is called once per frame
    void FixedUpdate()
    {
        int total = 0;
        for(int i = 0; i < npcs.Length; i++)
        {
            if(npcs[i] == null)
            {
                total++;
            }


        }

        if(total == npcs.Length)
        {
            record = Instantiate(loot, this.transform.position + new Vector3(0, .5f, 0), Quaternion.identity);
            record.GetComponent<Rigidbody>().isKinematic = true;

            //Instantiate(vfx ,  this.transform.position + new Vector3(0, .1f, 0), Quaternion.identity);

            Destroy(gameObject);

        }





    }
}
