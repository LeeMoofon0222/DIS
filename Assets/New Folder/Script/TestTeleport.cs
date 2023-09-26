using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTeleport : MonoBehaviour
{
    
    public GameObject StoneIsland;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if(other.gameObject.tag == "Player")
        {
            other.transform.position = StoneIsland.transform.position;


        }

    }
}
