using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HopeSpawnManger : MonoBehaviour
{

    public GameObject HopeObject;
    public GameObject SpawnPoint;
    List<GameObject> Hope = new List<GameObject>();
    public float spawndelay = 0;
    float deleteTime;
    public TextMeshProUGUI HopeText;
    public int HopeValue;

    void Awake()
    {
        deleteTime = 30f;
        spawndelay = Random.Range(10, 20);
    }

    void Update()
    {
        spawndelay -= Time.deltaTime;
        deleteTime -= Time.deltaTime;
        if (spawndelay < 0)
        {
            SpawnHope();
            spawndelay = Random.Range(10, 20);
        }
        if (deleteTime < 0)
        {
            foreach (var hope in Hope)
            {
                Destroy(hope);
            }
            Hope.Clear();
            deleteTime = 30f;
        }

        HopeText.text = HopeValue.ToString();
    }

    void SpawnHope()
    {
        int times = Random.Range(3, 11);

        for (int i = 0; i < times; i++)
        {
            var x = Random.Range(SpawnPoint.transform.position.x - 6.5f, SpawnPoint.transform.position.x + 6.5f);
            var z = Random.Range(SpawnPoint.transform.position.z - 6.8f, SpawnPoint.transform.position.z + 6.8f);
            var hope = Instantiate(HopeObject, new Vector3(x, SpawnPoint.transform.position.y, z), transform.rotation);
            Hope.Add(hope);
        }
    }
}
