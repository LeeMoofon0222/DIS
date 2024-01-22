using System.Collections;
using System.Collections.Generic;
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

    //[ColorUsage(true)]
    public Material lootColor;

}



public class MapLottery : MonoBehaviour
{
    BoxCollider boxCollider;
    public Transform generatePoint;

    [SerializeField]
    Collider[] colliders;

    bool reload;

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



    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
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


        if (Input.GetKeyDown(KeyCode.U))
        {
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

            Randomizor();
        }

    }

    void Randomizor()
    {

        int RandNum = Random.Range(1, 100);

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

                Material[] mats = mask1.gameObject.GetComponent<MeshRenderer>().materials;
                mats[0] = reward[i].lootColor;
                mask1.GetComponent<MeshRenderer>().materials = mats;

                StartCoroutine(WaitEffectes());

                if (reward[i].item[0] != null)
                {
                    if (reward[i].item.Count > 1)
                    {
                        int rand = Random.Range(0, reward[i].item.Count - 1);

                        Instantiate(reward[i].item[rand].spawntoscene, generatePoint.position, Quaternion.identity);


                    }
                    else
                    {
                        // m_inventory.AddItem(lootdrop[i].loot[0], 1, 1, 0);
                        Instantiate(reward[i].item[0].spawntoscene, generatePoint.position, Quaternion.identity);

                    }
                }

                break;
            }

        }

    }

    
    IEnumerator WaitEffectes()
    {

        yield return new WaitForSeconds(3f);
        Material[] mats = mask1.gameObject.GetComponent <MeshRenderer >().materials;
        mats[0] = mask1color;
        mask1.GetComponent<MeshRenderer>().materials = mats;



    }
    
}
