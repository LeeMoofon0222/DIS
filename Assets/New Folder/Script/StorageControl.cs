using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StorageControl : MonoBehaviour
{
    public StorageRecord storages;
    //public GameObject box;
    //public Transform point;
    public int storageNum;
    public int storageindex;
    

    private void Awake()
    {
        if (storages.Storages.Count == 0)
        {
            storages.Storages.Add(new Storage(storageindex));
            storages.Storages[0].assigned = true;
            return;
        }

        foreach (var storage in storages.Storages)
        {
            
            if (!storage.assigned)
            {
                storage.assigned = true;
                return;
            }
            storageNum++;
        }
        storages.Storages.Add(new Storage(storageindex));
        storages.Storages[storageNum].assigned = true;
    }

    private void OnDestroy()
    {
        storages.Storages[storageNum].assigned= false;

        foreach(var obj in storages.Storages[storageNum].Container)
        {
            obj.item = null;
            obj.amount = 0;
            obj.doneness = 0;
            obj.item_Health = 0;
        }
    }

    /*
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F))
        {
            Instantiate(box, point.position, Quaternion.identity);
            
        }
    }
    */
}
