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
    public GameObject plant;
    public List<GameObject> plantlist;
    private GameObject plantpoint;
    public void Update()
    {
        if (plant == null)
        {
            p += 10;
        }
        /*
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
        period = thisseed.period;
        plantlist = thisseed.plantstep;
        plantpoint = gameObject.transform.GetChild(0).gameObject;
        p = 0;
        plant = Instantiate(plantlist[p], plantpoint.transform.position, plantpoint.transform.rotation);
        plant.GetComponent<ItemObject>().canPick = false;
        plant.GetComponent<ItemObject>().PickObj = false;
        Destroy(plant, period);
        Invoke("Planting", period);
    }
    public void Planting()
    {
        p += 1;
        if (p < 3)
        {
            plant = Instantiate(plantlist[p], plantpoint.transform.position, plantlist[p].transform.rotation);
            plant.GetComponent<Rigidbody>().isKinematic = true;
            if (p < 2)
            {
                Destroy(plant, period);
            }
            Invoke("Planting", period);
        }
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