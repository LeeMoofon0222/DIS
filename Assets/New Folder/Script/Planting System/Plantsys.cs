using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    public Slider slider;
    private bool test1 = true;

    public void Start()
    {
        slider.gameObject.SetActive(true);
    }
    public void Update()
    {
        player = GameObject.FindWithTag("Player");
        watering = player.GetComponent<PlayerControl>().watering;
        //print(watering);
        if (watering == 1 && test1 && doplant)
        {
            //slider.gameObject.SetActive(true);
            Invoke("ReduceBar", 1);
            test1 = false;
        }

        if (doplant && watering == 1)
        {
            //gameObject.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().Play();
            Destroy(plant, period);
            Invoke("PlantPlant", period);
            player.GetComponent<PlayerControl>().watering = 0;
            doplant = false;
        }
        /*
        if(plant == null)
        {
            slider.gameObject.SetActive(false);
        }*/
        if (plant != null)
        {
            hasplant = true;
        }
        else
        {
            hasplant = false;
        }
        if (slider.value == 0)
        {
            slider.value = period;
            //slider.gameObject.SetActive(false);
        }
        if (p != 0 && p == plantlist.Count)
        {
            slider.gameObject.SetActive(false);
        }
        if (!doplant)
        {
            player.GetComponent<PlayerControl>().watering = 0;
        }
        if (!hasplant)
        {
            slider.gameObject.SetActive(true);
            slider.maxValue = period;
            slider.value = period;
        }
    }
    public void DoPlanting(PlantObject thisseed)
    {
        doplant = false;
        p = 0;
        period = thisseed.period;
        plantlist = thisseed.plantstep;
        slider.maxValue = period;
        step = plantlist.Count;
        id = thisseed.ID;

        if (id == 40001)
        {
            plantpoint = gameObject.transform.GetChild(0).gameObject;

        }
        if (id == 40002)
        {
            plantpoint = gameObject.transform.GetChild(1).gameObject;
        }
        if (id == 40003)
        {
            plantpoint = gameObject.transform.GetChild(1).gameObject;
        }
        if (id == 40004)
        {
            plantpoint = gameObject.transform.GetChild(1).gameObject;
        }
        if (id == 40005)
        {
            plantpoint = gameObject.transform.GetChild(0).gameObject;
        }
        plant = Instantiate(plantlist[p], plantpoint.transform.position, plantpoint.transform.rotation);
        plant.layer = 0;
        gameObject.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>().Play();
        planthealth = plant.AddComponent<ItemObject>().objectHealth;
        //print(planthealth);
        plant.GetComponent<Rigidbody>().isKinematic = true;
        plant.GetComponent<ItemObject>().canPick = false;
        plant.GetComponent<ItemObject>().PickObj = false;
        p += 1;
        doplant = true;
    }
    public void PlantPlant()
    {
        plant = Instantiate(plantlist[p], plantpoint.transform.position, plantlist[p].transform.rotation);
        test1 = true;
        gameObject.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>().Play();
        if (p < step - 1)
        {
            doplant = true;
        }
        p += 1;
    }

    public void ReduceBar()
    {
        slider.value -= 1;
        if (slider.value != 0)
        {
            Invoke("ReduceBar", 1);
        }
    }
}