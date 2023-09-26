using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMaterialControl : MonoBehaviour
{
    public GameObject obj;
    public Material mat;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("123");

            Material m = obj.GetComponent<Renderer>().material;
            m = mat;


        }
    }
}
