using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovetoObject : MonoBehaviour
{
    public Transform holder;


    // Update is called once per frame
    void Update()
    {
        this.transform.position = holder.position;
    }
}
