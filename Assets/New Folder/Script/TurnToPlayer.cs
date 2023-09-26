using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToPlayer : MonoBehaviour
{
    
    public Transform player;


    private void Update()
    {
        transform.LookAt(player);
    }
    void Start()
    {
       
    }
}