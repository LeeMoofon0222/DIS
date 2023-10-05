using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSpawnSO : MonoBehaviour
{
    public TestChestSave _item;
    public InventoryRecord ir;
    

    // Start is called before the first frame update
    void Start()
    {
        _item = ScriptableObject.CreateInstance<TestChestSave>();
    }

    private void Update()
    {

        GameObject Player = GameObject.FindGameObjectWithTag("Player");

        if (Input.GetKeyDown(KeyCode.I))
        {
            _item.items.Add(ir.Container[0].item);


        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Instantiate(_item.items[0].spawntoscene , Player.transform.position , Quaternion.identity);

        }


    }


}
