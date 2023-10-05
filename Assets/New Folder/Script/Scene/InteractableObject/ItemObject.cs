using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemObject : MonoBehaviour
{
    [Header("For Inventory(Scriptable Object)")]
    public Item item;

    [Header("For Prompt")]
    public string m_name;
    public Sprite m_sprite;


    [Header("For Interactable Object")]
    public float objectHealth;
    float maxOH;
    bool scalechanged1, scalechanged2;
    public bool CanScaleChange;
    public bool PickObj;        //可被撿起物資，判定是否自毀
    bool gotDig;
    public GameObject partical;
    //public Animator anim;

    [Header("Can PickUP")]
    public bool canPick;        //隨時可以被撿起

    [Header("Can Throw")]
    public bool PlayerCanThrow;
    public Rigidbody rb;

    public bool SceneSpawned;

    [Header("item properties")]
    public int record_health;
    public int record_doneness;


    ResourseGain resoursegain;
    FoodMaterialSetting FMS;

    Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        resoursegain = GetComponent<ResourseGain>();
        anim = GetComponent<Animator>();
    }

    void Awake()
    {
        maxOH = objectHealth;
        
        if (PlayerCanThrow && !SceneSpawned && !item.placeAble)
        {
            var playerHolder = GameObject.Find("grabItemHolder");
            //Debug.Log(playerHolder.transform.position);

            rb.AddForce(this.transform.forward * 2f , ForceMode.Impulse);

            if(item != null && item.type == ItemType.Food)
            {
                /*Material[] oh_Mats = this.gameObject.GetComponent<Renderer>().materials;       //代改
                FoodObject _item = (FoodObject)item;
                oh_Mats[0] = _item.roastStep[];
                this.gameObject.GetComponent<Renderer>().materials = oh_Mats;*/

                //Debug.LogWarning(record_doneness);
                print("test1");
                StartCoroutine(AwakeWait());

            }


        }
        if (SceneSpawned)
        {
            if(item != null)
            {
                record_health = item.itemHealth;
            }
        }
    }

    public void ObjectHealth(float num)
    {
        if(objectHealth > 0) objectHealth -= num;

        //if(!gotDig) StartCoroutine(animDig());
        //if (!gotDig) StartCoroutine(animDig());

        if (CanScaleChange)
        {
            if (objectHealth <= maxOH / 3 * 2 && !scalechanged1)
            {
                scalechanged1 = true;
                this.gameObject.transform.localScale = this.gameObject.transform.localScale / 6 * 5;
            }
            if (objectHealth <= maxOH / 3 && !scalechanged2)
            {
                scalechanged2 = true;
                this.gameObject.transform.localScale = this.gameObject.transform.localScale / 5 * 4;
            }
            

        }




        if (objectHealth <= 0 && !PickObj)
        {
            resoursegain.GetFinalResources();
            Destroy(gameObject);
        }

    }
    public void Update()
    {
        if(transform.position.y <= -20f) Destroy(gameObject);
    }
    /*IEnumerator animDig()
    {
        gotDig = true;
        anim.Play("Hit");
        yield return new WaitForSeconds(0.67f);
        anim.Play("None");
        gotDig = false;

    }*/

    IEnumerator AwakeWait()
    {
        yield return new WaitForEndOfFrame();
        FoodObject _food = (FoodObject)item;
        Material[] mats = this.gameObject.GetComponent<Renderer>().materials;
        mats[0] = _food.roastStep[record_doneness];
        GetComponent<Renderer>().materials = mats;


    }

    







}
