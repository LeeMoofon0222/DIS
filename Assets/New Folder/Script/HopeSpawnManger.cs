using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HopeSpawnManger : MonoBehaviour
{

    public List<GameObject> HopeObject;
    public GameObject SpawnPoint;
    List<GameObject> Hope = new List<GameObject>();
    public float spawndelay = 0;
   
    public TextMeshProUGUI HopeText;
    public int HopeValue;

    void Awake()
    {
        
        spawndelay = Random.Range(10, 20);
    }

    void Update()
    {
        spawndelay -= Time.deltaTime;
       
        if (spawndelay < 0)
        {
            SpawnHope();
            spawndelay = Random.Range(10, 20);
        }


        HopeText.text = HopeValue.ToString();
    }

    void SpawnHope()
    {
        int times = Random.Range(3, 11);

        for (int i = 0; i < times; i++)
        {
            var x = Random.Range(SpawnPoint.transform.position.x - 3.5f, SpawnPoint.transform.position.x + 3.5f);
            var z = Random.Range(SpawnPoint.transform.position.z - 3.8f, SpawnPoint.transform.position.z + 3.8f);
            int j = Random.Range(0, 3);
            var hope = Instantiate(HopeObject[j], new Vector3(x, SpawnPoint.transform.position.y, z), transform.rotation);
            Destroy(hope, 30f);
            
        }
    }
}
