using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;

public class Plantsys : MonoBehaviour
{
    public int p;
    private int period;
    private int id;
    private int step;
    public GameObject plant;
    private float planthealth;
    public List<GameObject> plantlist;
    private GameObject plantpoint;
    private GameObject player;
    public int watering;
    public bool hasplant = true;
    public bool doplant = false;
    public void Update()
    {
        player = GameObject.FindWithTag("Player");
        watering = player.GetComponent<PlayerControl>().watering;
        print(watering);
        if(doplant && watering == 1)
        {
            Destroy(plant, period);
            Invoke("PlantPlant", period);
            player.GetComponent<PlayerControl>().watering = 0;
            doplant = false;
        }
        if (plant != null)
        {
            hasplant = true;
        }
        else
        {
            hasplant = false;
        }
        /*
        if (plant == null)
        {
            p += 10;
        }
        for(int i = 0; i < Plantlist.Count; i++)
        {
            print(Plantlist[i]);
        }
        if (plant == null && p==1)
        {
            print(p);
            plant = Instantiate(Plantlist[1], plantpoint.transform.position, plantpoint.transform.rotation);
            plant.GetComponent<Rigidbody>().isKinematic = true;
            Destroy(plant, period);
            p++;
        }
        if (plant == null && p == 2)
        {
            plant = Instantiate(Plantlist[2], plantpoint.transform.position, plantpoint.transform.rotation);
            plant.GetComponent<Rigidbody>().isKinematic = true;
            p = 0;
        }*/
    }
    public void DoPlanting(PlantObject thisseed)
    {
        doplant = false;
        p = 0;
        period = thisseed.period;
        plantlist = thisseed.plantstep;
        step = plantlist.Count;
        id = thisseed.ID;

        if (id == 798789)
        {
            plantpoint = gameObject.transform.GetChild(0).gameObject;

        }
        if (id == 788888)
        {
            plantpoint = gameObject.transform.GetChild(1).gameObject;
        }
        plant = Instantiate(plantlist[p], plantpoint.transform.position, plantpoint.transform.rotation);
        gameObject.GetComponent<ParticleSystem>().Play();
        planthealth = plant.AddComponent<ItemObject>().objectHealth;
        print(planthealth);
        plant.GetComponent<Rigidbody>().isKinematic = true;
        plant.GetComponent<ItemObject>().canPick = false;
        plant.GetComponent<ItemObject>().PickObj = false;
        p += 1;
        doplant = true;
    }
    public void PlantPlant()
    {
        plant = Instantiate(plantlist[p], plantpoint.transform.position, plantlist[p].transform.rotation);
        gameObject.GetComponent<ParticleSystem>().Play();
        if (p < step-1)
        {
            doplant = true;
        }
        p += 1;
    }
}
/*private IEnumerator planting(Transform PlantPoint, int Plen, int period, List<GameObject> types, GameObject plant, int p)
    {
        GameObject j = Instantiate(plant, PlantPoint.position, PlantPoint.rotation);
        j.SetActive(true);
        //print(j.name);
        p += 1;
        if (p == Plen)
        {
            yield return 0;
        }
        else
        {
            print(period);
            yield return new WaitForSeconds(period);  // µ¥«Ý()¬í
            print(period);
            Destroy(j);
            plant = types[p];
            yield return planting(PlantPoint, Plen, period, types, plant, p);
        }
    }*/