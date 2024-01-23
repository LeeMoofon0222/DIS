using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGameObjectSpawn : MonoBehaviour
{
    public GameObject[] spawn;
    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < spawn.Length; i++)
        {
            spawn[i].SetActive(false);
        }
        
        spawn[Random.Range(0, spawn.Length)].SetActive(true);



        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
