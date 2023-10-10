using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static UnityEditor.Progress;


public class PlayerHealth : MonoBehaviour
{
    [Header("HealthBar")]
    //public static PlayerHealth Instance;
    public GameObject[] heart;
    public int currentlife;
    public int maxlife;

    public bool[] boolhealth;
    public GameObject LowhealthPanel;

    [Header("Awake")]
    public Image blackscreen;

    public Volume volume;
    Vignette vignette;
    ColorAdjustments exposure;
    float _exposure;

    public bool respawnable = false;

    [HideInInspector] public float timer;

    PlayerMoveMent pm;
    float u;

    bool respawned;

    [Header("Inventory")]
    public InventoryRecord inventory;
   

    // Start is called before the first frame update\
    void Awake()
    {
        //Instance = this;
        currentlife = maxlife;
        LowhealthPanel.SetActive(false);


        blackscreen.color = new Vector4(0, 0, 0, 255);
        pm = GetComponent<PlayerMoveMent>();
        volume.profile.TryGet<Vignette>(out vignette);
        vignette.intensity.value = 1;

        volume.profile.TryGet<ColorAdjustments>(out exposure);
        _exposure = exposure.postExposure.value;
        //print(_exposure);
        exposure.postExposure.value = 0f;

        respawnable = true;
    }
    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            boolhealth[i] = true;
            heart[i].gameObject.SetActive(true);

        }
        respawnable = true;
    }

    // Update is called once per frame
    public void Update()
    {

        if (currentlife > maxlife)
        {
            currentlife = maxlife;
        }

        for (int k = 1; k <= 20; k++)
        {
            if (currentlife < k)
            {
                heart[k - 1].gameObject.SetActive(false);
                boolhealth[k - 1] = false;
            }
        }
        for (int n = 0; n < 20; n++)
        {
            if (currentlife > n && currentlife < n + 2 && boolhealth[n] == false)
            {
                for (int a = 0; a < currentlife; a++)
                {
                    heart[a].gameObject.SetActive(true);

                    boolhealth[a] = true;
                }
            }
        }


        if (currentlife <= 6 && currentlife > 0)
        {
            LowhealthPanel.SetActive(true);
        }
        else
        {
            LowhealthPanel.SetActive(false);
        }

        if (respawnable)
        {
            
            timer += Time.deltaTime;
            blackscreen.color = new Vector4(0, 0, 0, Mathf.Lerp(1, 0, timer * 2));

            volume.profile.TryGet<Vignette>(out vignette);
            u = Mathf.Lerp(1, 0.15f, timer / Mathf.Lerp(1f, 3, timer * 1200));
            vignette.intensity.value = u;

            volume.profile.TryGet<ColorAdjustments>(out exposure);
            exposure.postExposure.value = Mathf.Lerp(0, _exposure, timer);
        }
        else
        {
            timer += Time.deltaTime;
            blackscreen.color = new Vector4(0, 0, 0, Mathf.Lerp(0, 1, timer * 3));
        }
    }


    public void Dead()
    {
        respawnable = false;
        if (!respawned)
        {
            if (inventory.Container.Count != 0)
            {
                int total = 0;
                foreach(var item in inventory.Container)
                {
                    total += item.amount;
                }

                for (int i = 0; i < Mathf.RoundToInt(total * 0.2f); i++)
                {
                    int rand = Random.Range(0, inventory.Container.Count);
                    Item _item = inventory.Container[rand].item;
                    int pnum = inventory.Container[rand].pNum;

                    GameObject envItem = Instantiate(_item.spawntoscene, this.transform.position + new Vector3(0,i* 2,0), Quaternion.identity);
                    if(envItem.TryGetComponent(out ItemObject io))
                    {
                        io.record_health = inventory.GetItemhealth(_item, pnum);
                        //io.item.canThrow = false;
                        if (io.item.type == ItemType.Food)
                        {
                            io.record_doneness = inventory.GetBBQStep(_item, pnum);
                        }
                    }
                    inventory.DecreesItem(_item, 1, pnum);
                }
            }



            respawned = true;
            timer = 0f;
            StartCoroutine(deadcountDown());
        }
    }



    public void increase_health(int value)
    {
        currentlife += value;
    }
    public void TakeDamage(int value)
    {
        currentlife -= value;

        if (currentlife < 1)
        {
            Dead();
        }
    }

    IEnumerator deadcountDown()
    {
        //pm.canMove = false;
        yield return new WaitForSeconds(2f);
        timer = 0f;

        blackscreen.color = new Vector4(0, 0, 0, 255);

        volume.profile.TryGet<Vignette>(out vignette);
        vignette.intensity.value = 1;


        volume.profile.TryGet<ColorAdjustments>(out exposure);
        exposure.postExposure.value = 0f;

        pm.cC.enabled = false;
        pm.player.transform.position = pm.respawnPoint;
        pm.cC.enabled = true;

        respawnable = true;
        pm.isDead = false;
        currentlife = maxlife;

        respawned = false; 
        //pm.canMove = true;
    }
}
