using System.Collections;
using System.Collections.Generic;
using DitzeGames.Effects;

using UnityEngine;

[System.Serializable]
public class points
{
    public int ID;
    public int weight;

}

[System.Serializable]
public class LootandWeight
{
    public List<Item> item;
    public int needpoint;
    public int weight;

    [ColorUsage(true,true)]
    public Color lootColor;

}



public class MapLottery : MonoBehaviour
{
    BoxCollider boxCollider;
    public Transform generatePoint;

    [SerializeField]
    Collider[] colliders;

    bool isworking;

    public LayerMask detections;

    public List<IDandAmount> items;
    //public List<Transform> floating_pos;

    public List<points> point;
    public int totalpoint;

    Dictionary<Item, int> recorded = new Dictionary<Item, int>();
    int pos;

    public List<LootandWeight> reward;
    public GameObject mask1;
    public Material mask1color;
    [ColorUsage(true, true)]
    public Color og_color;
    public Animator maskAnimator;

    public Animator glowing;

    PlayerMoveMent PM;

    public ParticleSystem particle;

    CameraEffects CE;



    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isworking)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            PM = player.GetComponent<PlayerMoveMent>();
            PM.canMove = false;
        }


        colliders = new Collider[0];

        recorded.Clear();
        items.Clear();
        pos = 0;

        colliders = Physics.OverlapBox(this.transform.position, boxCollider.size / 2, Quaternion.identity, detections);

        for (int i = 0; i < colliders.Length; i++)
        {
            Item item = colliders[i].GetComponent<ItemObject>().item;
            IDandAmount sets = new IDandAmount(item, 1, -1, null);

            if (items.Count != 0)
            {
                if (!recorded.ContainsKey(item))
                {
                    items.Add(sets);
                    recorded.Add(item, pos);
                    pos++;
                }
                else
                {
                    //print(recorded[item]);
                    items[recorded[item]].amount += 1;

                }
            }
            else
            {
                items.Add(sets);
                recorded.Add(item, pos);
                pos++;
            }




        }


        if (Input.GetKeyDown(KeyCode.U) && !isworking)
        {
            totalpoint= 0;
            for(int i = 0; i < items.Count; i++)
            {
                
                for (int j = 0; j < point.Count; j++)
                {
                    if (items[i].itemID == point[j].ID)
                    {
                        totalpoint += point[j].weight * items[i].amount;
                        break;
                    }

                }

            }

            DestroyItem();
            Randomizor();
        }

    }

    public void SystemStart()
    {
        if (!isworking)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            PM = player.GetComponent<PlayerMoveMent>();
            PM.canMove= false;

            CE = GameObject.Find("PlayerCamera").GetComponent<CameraEffects>();

            totalpoint = 0;
            for (int i = 0; i < items.Count; i++)
            {
                for (int j = 0; j < point.Count; j++)
                {
                    if (items[i].itemID == point[j].ID)
                    {
                        totalpoint += point[j].weight * items[i].amount;
                        break;
                    }

                }
            }

            if(totalpoint != 0)
            {
                DestroyItem();
                Randomizor();

            }
        }
    }


    void DestroyItem()
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            Destroy(colliders[i].gameObject);
        }

    }



    void Randomizor()
    {
        if (isworking) return;

        int RandNum = Random.Range(1, 100);
        int rec = RandNum;

        for (int i = 0; i < reward.Count; i++)
        {
            if (RandNum > reward[i].weight)
            {
                RandNum -= reward[i].weight;
            }
            else
            {
                if(totalpoint < reward[i].needpoint)
                {
                    Randomizor();
                    return;
                }

                //Material[] mats = mask1.gameObject.GetComponent<Material>().colo;
                //mats[0] = reward[i].lootColor;
                //mask1.GetComponent<MeshRenderer>().materials = mats;

                //mask1color.SetColor("_EmissionColor", reward[i].lootColor);

                StartCoroutine(WaitEffectes());
                maskAnimator.SetInteger("Color", i);

                if (reward[i].item[0] != null)
                {
                    
                    if (reward[i].item.Count > 1)
                    {
                        int rand = Random.Range(0, reward[i].item.Count - 1);

                        if(RandNum <= 52 && rand != 1)
                        {
                            GameObject op = Instantiate(reward[i].item[rand].spawntoscene, generatePoint.position, Quaternion.identity);
                            op.GetComponent<ItemObject>().SceneSpawned = true;
                        }
                        else
                        {
                            for(int k = 0; k <= RandNum % 3; k++)
                            {
                                GameObject op = Instantiate(reward[i].item[rand].spawntoscene, generatePoint.position + new Vector3(k / 6, 0, 0), Quaternion.identity);
                                op.GetComponent<ItemObject>().SceneSpawned = true;
                            }
                        }
                        


                    }
                    else
                    {
                        // m_inventory.AddItem(lootdrop[i].loot[0], 1, 1, 0);
                        if(rec >= 96)
                        {
                            GameObject op = Instantiate(reward[i].item[0].spawntoscene, generatePoint.position, Quaternion.identity);
                            op.GetComponent<ItemObject>().SceneSpawned = true;
                        }
                        else
                        {
                            for (int k = 0; k <= RandNum % 3; k++)
                            {
                                GameObject op = Instantiate(reward[i].item[0].spawntoscene, generatePoint.position + new Vector3(k/6,0,0), Quaternion.identity);
                                op.GetComponent<ItemObject>().SceneSpawned = true;
                            }
                        }

                        
                        

                    }
                }

                break;
            }

        }

    }

    
    IEnumerator WaitEffectes()
    {
        glowing.Play("Glowing");
        maskAnimator.SetTrigger("Glow");
        particle.Play();

        CE.Shake(5,1);

        isworking = true;
        yield return new WaitForSeconds(6f);

        //Material[] mats = mask1.gameObject.GetComponent <MeshRenderer >().materials;
        //mats[0] = mask1color;
        //mask1.GetComponent<MeshRenderer>().materials = mats;
        mask1color.SetColor("_EmissionColor", og_color);

        //Instantiate()

        isworking= false;
        if(PM != null) PM.canMove = true;

    }
    
}
