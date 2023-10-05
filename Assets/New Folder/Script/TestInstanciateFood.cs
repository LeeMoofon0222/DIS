using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInstanciateFood : MonoBehaviour
{
    public InventoryRecord inv;

    public FoodObject food;
    public FoodObject References;
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            //food = FoodObject.CreateInstance<FoodObject>();
            food = Instantiate(References);
            //food.testing = 10;



        }

    }
}
