using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgingRotator : MonoBehaviour
{
    void Awake()
    {
        int r = Random.Range(1, 4) ;

        this.transform.localRotation = Quaternion.Euler(0, r * 90, 0);
        //this.GetComponent<BridgingRotator>().enabled = false;
    }
}
