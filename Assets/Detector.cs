using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{

    public MapLottery lottery;


    private void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag("Player"))
        {
            //transform.localPosition = transform.localPosition + new Vector3(0, -0.03f, 0);
            lottery.SystemStart();
        }
    }



}
