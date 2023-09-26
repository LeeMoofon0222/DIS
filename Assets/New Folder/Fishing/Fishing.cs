using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    public float smooth =3f;
    public bool waterTrigger;
    public bool getFish = false;
    public GameObject hook;
    public Vector3 pos;
    public float Dtime;
    private bool otherTrigger;
    public float FishingTime;

    public Transform initialPos;

    public bool Ihavecollisth;
    public bool canFish;
    public bool isFishing;

    public float h_timer;


    [Header("Hope")]
    public int HopeValue;
    public GameObject HopeObject;
    public Text HopeText;







    public void Awake()
    {
        canFish = true;
        HopeText = transform.Find("HopeValue").GetComponent<Text>();
    }
    public void Update()
    {
        HopeText.text = HopeValue.ToString();
        var pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        if (waterTrigger)
        {
            FishingTime -= Time.deltaTime;
            hook.transform.position = pos;
            if(FishingTime > 0 && Input.GetMouseButtonDown(1))
            {
                waterTrigger = false;
            }
            if (FishingTime <= 0)
            {
                //這裡放魚鉤被魚咬特效
                //hook.transform.position.y -= 1;
                Debug.Log("上鉤");
                Dtime -= Time.deltaTime;
                
                if (Dtime >= 0 && Input.GetMouseButtonDown(1))//Get the fish
                {
                    Vector3 pos = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
                   
                    int d = Random.Range(3, 11);
                    for(int i=0; i<d; i++)
                    {
                        int a = Random.Range(-2, 3);
                        int b = Random.Range(-2, 3);
                        int c = Random.Range(-2, 3);
                        Instantiate(HopeObject, new Vector3(pos.x + a, pos.y + b, pos.z + c), transform.rotation);
                    }
                    
                    Dtime = 2f;
                    FishingTime = Random.Range(2, 4);
                    getFish = true;
                    waterTrigger = false;
                }
                else if(Dtime<0)
                {
                    FishingTime = Random.Range(2, 4);
                    //getFish = false;
                    Dtime = 2f;
                    print("ur gay");
                    //hook.transform.position.y += 1;
                }
            } 
        }
        else if(otherTrigger)
        {
            hook.transform.position = pos;
            if (Input.GetMouseButtonDown(1))
            {
                otherTrigger = false;
            } 
        }
        /*
        h_timer += Time.deltaTime;

        if (h_timer > 7f)
        {
            hook.GetComponent<Rigidbody>().useGravity = false;

            hook.GetComponent<Rigidbody>().isKinematic = true;
            hook.GetComponent<Rigidbody>().isKinematic = false;

            pc.fishingPullBack = true;
            hook.transform.position = initialPos.transform.position;
        }*/
        if (Vector3.Distance(hook.transform.localPosition, initialPos.transform.localPosition) >= 6000f)
        {
            //print("ff");
            hook.GetComponent<Rigidbody>().useGravity = false;
            hook.GetComponent<Rigidbody>().isKinematic = true;
            hook.transform.position = initialPos.transform.position;
            hook.GetComponent<Rigidbody>().isKinematic = false;
            canFish = true;
        }

    }
    void OnTriggerEnter(Collider other)
    {
        
        var h = GetComponent<Rigidbody>();
        h.isKinematic = true;
        Ihavecollisth = true;
        if (other.gameObject.tag == "pond")
        {
            //這裡放魚鉤進水特效
            waterTrigger = true;
            pos = hook.transform.position;
            hook.GetComponent<BoxCollider>().enabled = false;
            //Debug.LogWarning("WaterTriggered");
        }
        else
        {
            otherTrigger = true;
            hook.GetComponent<BoxCollider>().enabled = false;
            pos = hook.transform.position;
        }
    }

    public void fishing(GameObject hook)
    {
        FishingTime = Random.Range(2, 4);
        Dtime = 2f;
        var hook_rb = hook.GetComponent<Rigidbody>();
        hook_rb.useGravity = true;
        var cam = GameObject.Find("IslandHolder");
        var player = GameObject.Find("Player V1.6");
        hook_rb.AddForce(new Vector3((cam.transform.position.x-player.transform.position.x)*2f,
                                     (cam.transform.position.y-player.transform.position.y)*2f, 
                                     (cam.transform.position.z- player.transform.position.z)*2f) , ForceMode.Impulse);
    }
}
