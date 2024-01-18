using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map_craft : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] collider = Physics.OverlapBox(gameObject.transform.position, new Vector3(3, 3, 3), Quaternion.identity, LayerMask.GetMask("Enemy"));
        foreach (var col in collider)
        {
            Debug.Log(col);
        }
    }
}
