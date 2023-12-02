using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class PlayerControl : MonoBehaviour
{
    public bool AbsoluteControlRotate = false;
    public void AbsoluteControlRotateButton(bool n)
    {
        AbsoluteControlRotate = n;
    }

    //for setting
    public PlayerCameraLook cameraControl;
    public Animator handHolderController;
    //public Animator camHolderController;
    public bool canDig;
    public bool attacking;
    [SerializeField] float digCD;
    public bool fishingPullBack;
    public int watering = 0;
    float timer;
    //bool isDigging;

    public PlayerMoveMent pm;
    public InventoryRecord m_inventory;
    public barController bc;
    public ItemwheelController IWC;

    public GameObject plantpoint;

    public Transform cam;
    RaycastHit hit;
    public float maxRange;

    float nowItem;

    public ItemObject itemHolding;
    public GameObject onHandItem;

    public Transform ItemHolder;


    public int setItem;


    bool uiSwitching;

    GameObject preplaceObj;

    

    [Header("eating")]
    bool iseating;
    [SerializeField] float eatCD;

    [Header("Game Manager")]
    public SystemsManager systemsManager;

    [Header("Inventory / Change your item")]
    public PlayerInventoryController PIC;
    //public GameObject mainHandItem;
    public Transform mainhandHolder;
    public bool onhandSpawned;      //主手重製
    public variables onhandProperty;
    

    [Header("other option panel")]
    public GameObject optionMenu;
    public bool optionPage;

    [Header("Roasting")]
    public bool bbq;
    public LayerMask bbqLayer;
    bool bbq_isPlayed;
    float bbqTimer;
    public GameObject BBQsmoke;
    bool animPlayed;

    [Header("Cursor")]
    public Image cursor;

    [Header("Storage")]
    public StorageRecord storages;

    [Header("Audio")]
    public AudioClip pickClip;
    public AudioSource playerSource;
    public AudioSource swing;

    public void SettingmodeForRotate(bool n)
    {
        optionPage = n;
    }

    public bool anyoptionOn()
    {
        foreach(var sys in systemsManager.systems)
        {
            if (sys.activeInHierarchy)
            {
                return true;
                
            }
        }
        return false;
    }
    public GameObject lefthandHolder;

    public Image selectImage;

    /*[Header("For Building System")]
    public GameObject BS;
    //bool switch_CBS;
    public bool CallBuildSystem()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                return true;
            }
        }
        return false;
    }*/
    
    ResourseGain extraResources;

    // Start is called before the first frame update
    void Awake()
    {
        
        //isDigging = false;
        handHolderController.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        RaycastHit hitInfo;

        selectImage.sprite = PIC.slotIcon[setItem].sprite;

        exiting();

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, maxRange))
        {
            if(hitInfo.transform.TryGetComponent(out ItemObject IO))
            {
                cursor.GetComponent<Image>().color = Color.green;
            }
            else
            {
                cursor.GetComponent<Image>().color = Color.white;
            }
        }
        else
        {
            cursor.GetComponent<Image>().color = Color.white;
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            optionMenu.SetActive(true);
            optionPage = true;

        }
        if(optionMenu.activeInHierarchy) 
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        lefthandHolder.SetActive(!anyoptionOn());
        


        /*if (CallBuildSystem())
        {
            BS.SetActive(!BS.activeInHierarchy);
        }*/

        if (!uiSwitching && !anyoptionOn())
        {
            float mw = Input.GetAxis("Mouse ScrollWheel");
            nowItem += mw * 10f;
            nowItem = Mathf.Repeat(nowItem, PIC.itemsOnHand.Count - 0.5f);
            setItem = Mathf.RoundToInt(nowItem);
        }

        if (AbsoluteControlRotate)
        {
            cameraControl.canRotate = false;
        }
        else
        {
            cameraControl.canRotate = !optionPage;  //Camera
        }

        ChangeonHandItem();

        if(onHandItem != null)
        {
            onhandProperty.itemhealth = PIC.itemProperties[setItem].itemhealth;
            onhandProperty.itemdoneness = PIC.itemProperties[setItem].itemdoneness;
            
        }
        else
        {
            itemHolding = null;
        }

        /*#region Fishing

        if (onHandItem != null && onHandItem.GetComponent<ItemObject>().item.ID == 21000)
        {
            GameObject hook = onHandItem.transform.GetChild(0).gameObject;
            var hrb = hook.GetComponent<Rigidbody>();
            var fishing = hook.GetComponent<Fishing>();
            if (Input.GetMouseButtonDown(1) && !fishingPullBack)
            {
                //UnityEngine.Debug.Log("收");
                hrb.isKinematic = false;
                fishingPullBack = true;
                //碰到東西關掉RB
            }
            if (fishing.getFish)
            {
                //UnityEngine.Debug.LogWarning("yougetfish");
                fishing.getFish = false;
            }

            if (fishingPullBack && fishing.Ihavecollisth)
            {
                print("fpb");
                hrb.isKinematic = false;
                hrb.useGravity = false;
                hook.transform.localPosition = Vector3.Lerp(hook.transform.localPosition, fishing.initialPos.transform.localPosition, fishing.smooth * Time.deltaTime);
                if (Vector3.Distance(hook.transform.localPosition, fishing.initialPos.transform.localPosition) <= 1f)
                {
                    fishing.waterTrigger = false;
                    fishing.canFish = true;
                    hook.GetComponent<BoxCollider>().enabled = true;
                    fishing.Ihavecollisth = false;
                    print("back");
                }
            }
        }

        if (onHandItem != null && onHandItem.GetComponent<ItemObject>().item.ID == 21000 && Input.GetMouseButtonDown(1) && onHandItem.transform.GetChild(0).gameObject.GetComponent<Fishing>().canFish)
        {
            GameObject hook = onHandItem.transform.GetChild(0).gameObject;
            var _fishing = hook.GetComponent<Fishing>();
            _fishing.fishing(hook);
            _fishing.canFish = false;
            fishingPullBack = false;
        }
        #endregion*/

        #region Placing
        if (itemHolding != null && itemHolding.item.placeAble)
        {
            if(Physics.Raycast(cam.transform.position, cam.transform.forward,out RaycastHit pointHit, maxRange, LayerMask.GetMask("islands")))
            {
                if ( GameObject.FindGameObjectWithTag("preplace") == null)
                {
                    preplaceObj = Instantiate(itemHolding.item.Preplace , pointHit.point + pointHit.normal * 0.00001f, Quaternion.identity);
                }
                if(preplaceObj != null)preplaceObj.transform.position = pointHit.point;
                preplaceObj.transform.up = pointHit.normal;

                if (Input.GetMouseButtonDown(1))
                {
                    Instantiate(itemHolding.item.spawntoscene, pointHit.point, preplaceObj.transform.rotation );
                    m_inventory.DecreesItem(itemHolding.item, 1, PIC.i_pnum[setItem]);

                    PIC.UpdateDisplay();
                }
            }
            else
            {
                GameObject[] delPre = GameObject.FindGameObjectsWithTag("preplace");
                foreach (var obj in delPre)
                {
                    Destroy(obj);
                }
            }
        }
        #endregion

        //onHand = m_inventory.Container[0].item.itemObject;
        #region Q_trhow
        if (m_inventory.Container.Count != 0)
        {
            if (PIC.itemsOnHand[setItem] != null)
            {

                itemHolding = PIC.itemsOnHand[setItem].itemObject.GetComponent<ItemObject>();
                if (Input.GetKeyDown(KeyCode.Q) && itemHolding != null && itemHolding.item.canThrow && !systemsManager.systems[0].activeInHierarchy && !systemsManager.systems[1].activeInHierarchy)
                {
                    GameObject itm = Instantiate(itemHolding.item.spawntoscene, ItemHolder.position, ItemHolder.rotation);

                    var io = itm.GetComponent<ItemObject>();
                    if(io != null)
                    {
                        io.record_health = m_inventory.GetItemhealth(itemHolding.item, PIC.i_pnum[setItem]);

                        if (io.item.type == ItemType.Food)
                        {
                            io.record_doneness = m_inventory.GetBBQStep(itemHolding.item, PIC.i_pnum[setItem]);
                        }
                    }


                    m_inventory.DecreesItem(itemHolding.item, 1 , PIC.i_pnum[setItem]);
                    PIC.UpdateDisplay();

                }
            }
            else
            {
                itemHolding = null;     //最遠(早)的itemHolding 程式
            }

        }


        #endregion

        if (PIC.itemsOnHand[setItem] == null)
        {
            onhandSpawned = false;
        }



        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f));
        #region Planting
        if (Input.GetMouseButtonDown(1) && Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit raycastHit, maxRange))
        {
            if (raycastHit.transform.tag == ("field"))
            {
                GameObject thisfield = raycastHit.transform.gameObject;
                bool hasplant = thisfield.GetComponent<Plantsys>().hasplant;
                plantpoint = raycastHit.transform.gameObject.transform.GetChild(0).gameObject;
                if (onHandItem.CompareTag("watering-can") && hasplant)
                {
                    thisfield.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().Play();
                    watering = 1;
                }
                else
                {
                    watering = 0;
                }
                if (onHandItem != null && onHandItem.GetComponent<ItemObject>().item.type == ItemType.Plant && thisfield.GetComponent<Plantsys>().plant==null)
                {
                    PlantObject thisseed = (PlantObject)onHandItem.GetComponent<ItemObject>().item;
                    if (m_inventory.Container.Count != 0)
                    {
                        if (PIC.itemsOnHand[setItem] != null)
                        {
                            itemHolding = PIC.itemsOnHand[setItem].itemObject.GetComponent<ItemObject>();
                            if (itemHolding != null && !systemsManager.systems[0].activeInHierarchy && !systemsManager.systems[1].activeInHierarchy)
                            {
                                m_inventory.DecreesItem(itemHolding.item, 1, PIC.i_pnum[setItem]);
                            }
                        }
                    }
                    thisfield.GetComponent<Plantsys>().DoPlanting(thisseed);
                    //yield return planting(PlantPoint, Plen, period, types, plant, p);
                    //planting(pp.transform, Plen, thisseed.period, Plantlist, onHandItem, p)
                }
            }
        }

        #endregion

        #region ATK/Mineing
        if (Input.GetMouseButton(0) && timer >= digCD && !anyoptionOn() && !PIC.inventoryOpen && !IWC.weaponWheelSelected)
        {
            StopAllCoroutines();
            swing.Play();
            StartCoroutine(_Digging());
            attacking = true;
            
            
            timer = 0;

            if (Physics.Raycast(ray,out hit,maxRange))
            {
                if (hit.transform.TryGetComponent(out ItemObject IO))
                {
                    var gain = IO.transform.GetComponent<ResourseGain>();
                    var hitSound = IO.transform.GetComponent<AudioSource>();
                    if (!IO.canPick)
                    {
                        if(IO.partical != null)
                        {
                            GameObject particalPrefab = Instantiate(IO.partical, hit.point + hit.normal * 0.001f, Quaternion.identity);
                            particalPrefab.transform.LookAt(hit.point + hit.normal);
                        }

                        if (IO.item != null)     //判斷有沒有帶Item (他是有意義的)
                        {
                            if(onHandItem != null && onHandItem.GetComponent<ItemObject>().item.gainingType == gain._forRatioType )
                            {
                                gain.GainMoreResources((ToolObject)onHandItem.GetComponent<ItemObject>().item);
                                //m_inventory.AddItem(IO.item, 1);
                                gain.tryGetExtraResources();
                                //Debug.Log("detected");
                            }
                            else
                            {
                                
                                if(gain.baseItem != null )m_inventory.AddItem(gain.baseItem, gain.peritemuGet, IO.item.itemHealth , 0);
                            }
                        }
                        IO.ObjectHealth(10);
                        
                        //hitSound.PlayOneShot(IO.item.hitsound);
                        



                        if (itemHolding != null && itemHolding.item.type == ItemType.Tool)
                        {
                            m_inventory.breakingItem(itemHolding.item, 1, PIC.i_pnum[setItem]);     //0323
                        }
                        
                    }


                }
            }
        }
        else
        {
            attacking = false;
        }
        #endregion

        #region furnance
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, maxRange))
        {
            if (hitInfo.transform.TryGetComponent(out Smelter smelter))
            {
                //Debug.Log("detected");
                if (Input.GetMouseButtonDown(1) && itemHolding.item != null && !smelter.issmelting)
                {
                    //smelter.Smelt(itemHolding.item, m_inventory.GetItemAmount(itemHolding.item));
                    smelter.setToSmelt(itemHolding.item);
                    PIC.UpdateDisplay();
                }
            }
        }
        #endregion

        #region PickUp
        if (Physics.Raycast(ray , out hit, maxRange))
        {
            if(hit.transform.TryGetComponent(out ItemObject IO))
            {
                if (Input.GetKeyDown(KeyCode.F) && !systemsManager.systems[0].activeInHierarchy && !systemsManager.systems[1].activeInHierarchy && IO.canPick)
                {
                    if (IO.SceneSpawned)
                    {
                        m_inventory.AddItem(IO.item, 1, IO.item.itemHealth, IO.record_doneness);
                    }
                    else
                    {
                        m_inventory.AddItem(IO.item, 1, IO.record_health, IO.record_doneness);
                    }
                    
                    Destroy(IO.transform.gameObject);

                    playerSource.PlayOneShot(pickClip);
                }
            }
        }
        #endregion

        #region StorageSystem
        if (Physics.Raycast(ray, out hit, maxRange))
        {
            if (hit.transform.TryGetComponent(out StorageControl storageCtrl))
            {
                if (!systemsManager.systems[0].activeInHierarchy && !systemsManager.systems[1].activeInHierarchy )
                {
                    //Debug.Log("FoundStorage");
                    PIC.isStorage= true;
                    PIC.storageNum = storageCtrl.storageNum;
                }
            }
            else
            {
                PIC.isStorage = false;
            }
        }

        #endregion

        /*if (Input.GetMouseButtonUp(1) || 
            Input.GetMouseButton(0) || 
            Input.GetKey(KeyCode.L) || 
            Input.GetKey(KeyCode.E) || 
            Input.GetKey(KeyCode.Q) || 
            Input.GetKey(KeyCode.F))
        {
            bbqstateSave = false;
        }*/
        //Debug.Log(bbqTimer);
        if (onHandItem != null && onHandItem.TryGetComponent(out ItemObject food_IO) && food_IO.item.type == ItemType.Food)
        {
            var food = (FoodObject)food_IO.item;
            if (onHandItem.GetComponent<ItemObject>().item.type == ItemType.Food)
            {
                //GameObject fork = onHandItem.transform.GetChild(0).gameObject;

                if (Input.GetMouseButton(1) && Physics.Raycast(cam.transform.position, cam.transform.forward, maxRange, bbqLayer) && food.canbeBBQ)    
                {
                    
                    bbqTimer += Time.deltaTime;
                    for(int i = m_inventory.GetBBQStep(itemHolding.item, PIC.i_pnum[setItem]);i< food.stepTime.Count; i++)
                    {
                        if (food.stepTime[i] <= bbqTimer)
                        {
                            //print(bbqTimer);
                            /*Material[] Mats = food.spawntoscene.GetComponent<Renderer>().sharedMaterials;
                            Mats[0] = food.roastStep[i];
                            food.spawntoscene.GetComponent<Renderer>().sharedMaterials = Mats;
                            */
                            Material[] oh_Mats = onHandItem.GetComponent<Renderer>().materials;       //代改
                            oh_Mats[0] = food.roastStep[i];
                            onHandItem.GetComponent<Renderer>().materials = oh_Mats;

                            m_inventory.SetBBQStep(itemHolding.item, i, PIC.i_pnum[setItem]);

                            /*Destroy(onHandItem.gameObject);     //刷新
                            onHandItem = Instantiate(PIC.itemsOnHand[setItem].itemObject, mainhandHolder);
                            onHandItem.GetComponent<Rigidbody>().isKinematic = true;*/
                        }
                    }
                    bbq = true;

                    StartCoroutine(smoking());
                    //fork.SetActive(true);
                    if (!bbq_isPlayed)
                    {
                        handHolderController.Play("Roasting");
                        
                        bbq_isPlayed = true;
                    }
                    
                }
                else if (Input.GetMouseButtonDown(1) && !iseating && !anyoptionOn() && !PIC.inventoryOpen && !bbq)
                {
                    if (onHandItem.GetComponent<ItemObject>().item.type == ItemType.Food)
                    {
                        StartCoroutine(_eating(food));
                    }
                    
                }

                if ((Input.GetMouseButtonUp(1) ||           
                    Input.GetMouseButton(0) ||
                    Input.GetKey(KeyCode.L) ||
                    Input.GetKey(KeyCode.E) ||
                    Input.GetKey(KeyCode.Q) ||
                    Input.GetKey(KeyCode.F) ||
                    Input.GetKey(KeyCode.Tab)) && bbq)
                {
                    //fork.SetActive(false);
                    if (pm.GetComponent<PlayerMoveMent>().isMoving)
                    {
                        handHolderController.Play("moving");
                    }
                    else
                    {
                        handHolderController.Play("Idle");
                    }

                    bbq = false;
                    if (bbq_isPlayed)
                    {
                        GameObject smoke = Instantiate(BBQsmoke, onHandItem.transform.position, Quaternion.identity);
                        Destroy(smoke, 6f);
                    }
                    bbq_isPlayed = false;
                    //Debug.Log(m_inventory.GetBBQStep(itemHolding.item, PIC.i_pnum[setItem]));
                    bbqTimer = food.stepTime[m_inventory.GetBBQStep(itemHolding.item, PIC.i_pnum[setItem])];

                }
            }
        }  
        
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, maxRange))
        {
            if(hitInfo.transform.TryGetComponent(out CubeMove cubemove))
            {

                cubemove.trigger();
                
            }
        }


    }

    public void ChangeonHandItem()      //主手物體控制
    {
        if (!onhandSpawned)     //取決於我手上是不是空的(我有沒有拿東西 或是不該拿東西的時候有拿東西)
        {
            GameObject[] delPre = GameObject.FindGameObjectsWithTag("preplace");
            foreach (var obj in delPre)
            {
                Destroy(obj);
            }


            if (mainhandHolder.childCount != 0)      //清除
            {
                foreach (Transform child in mainhandHolder)
                {
                    if(!child.transform.CompareTag("hand-dont-delete"))
                        Destroy(child.gameObject);
                }
                onhandProperty.itemhealth = 0;
                onhandProperty.itemdoneness= 0;
            }
            //Debug.Log(setItem);
            
            if(PIC.itemsOnHand[setItem] != null)   //補上     
            {
                onHandItem =  Instantiate(PIC.itemsOnHand[setItem].itemObject, mainhandHolder);
                onHandItem.GetComponent<Rigidbody>().isKinematic = true;

                onhandProperty.itemhealth = PIC.itemProperties[setItem].itemhealth;
                onhandProperty.itemdoneness = PIC.itemProperties[setItem].itemdoneness;
            }

            onhandSpawned = true;

            if(onHandItem != null)
            {
                if (pm.GetComponent<PlayerMoveMent>().isMoving)
                {
                    handHolderController.Play("moving");
                }
                else
                {
                    handHolderController.Play("Idle");
                }
                StopAllCoroutines();


                var IO = onHandItem.GetComponent<ItemObject>();
                IO.record_health = onhandProperty.itemhealth;

                if (IO.item.type == ItemType.Food)
                {
                    FoodObject _food = (FoodObject)IO.item;
                    

                    //Debug.Log(m_inventory.GetBBQStep(IO.item, PIC.i_pnum[setItem]));
                    IO.record_doneness = onhandProperty.itemdoneness;
                    if(_food.canbeBBQ) bbqTimer = _food.stepTime[m_inventory.GetBBQStep(IO.item, PIC.i_pnum[setItem])];

                }
            }

        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)        //滾輪移動後重製
        {
            RepairItemOnHand();
        }

    }

    public void RepairItemOnHand()      //重置主手
    {
        onhandSpawned = false;
    }

    public void SetOnhandItem(int set)      //給坐UI的智障
    {
        //Debug.Log("gay");     
        nowItem = set;
        RepairItemOnHand();

    }

    public void exiting()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionMenu.SetActive(false);
            foreach (var system in systemsManager.systems)
            {
                system.SetActive(false);
            }
            PIC.inventoryOpen = false;
            IWC.weaponWheelSelected = false;


            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            optionPage = false;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(cam.transform.forward, cam.transform.position + cam.transform.forward * 6f);
    }

    #region PlayAnimator
    IEnumerator _Digging()
    {
        //isDigging = true;
        handHolderController.Play("Digging");
        yield return new WaitForSeconds(0.67f);
        if (pm.GetComponent<PlayerMoveMent>().isMoving)
        {
            handHolderController.Play("moving");
        }
        else
        {
            handHolderController.Play("Idle");
        }
        //isDigging = false;
    }

    #endregion

    IEnumerator _eating(FoodObject food)
    {
        handHolderController.Play("Eating");
        yield return new WaitForSeconds(1.7f);
        
        bc.EatFood(Mathf.RoundToInt(food.satiety));

        bc.Setenergy(100);
        if(food.ID < 31000)
        {
            m_inventory.DecreesItem(itemHolding.item, 1, PIC.i_pnum[setItem]);
        }
        else
        {
            m_inventory.breakingItem(itemHolding.item, 1, PIC.i_pnum[setItem]);
        }
        

        yield return new WaitForSeconds(0.4f);

        if (pm.GetComponent<PlayerMoveMent>().isMoving)
        {
            handHolderController.Play("moving");
        }
        else
        {
            handHolderController.Play("Idle");
        }

    }

    IEnumerator smoking()
    {
        if (!animPlayed)
        {   
            animPlayed = true;
            yield return new WaitForSeconds(2f);
            GameObject smoke = Instantiate(BBQsmoke, onHandItem.transform.position, Quaternion.identity);
            Destroy(smoke , 6f);
            animPlayed = false;
        }

    }

    public void UIrotateLock(bool tf)
    {
        optionPage = tf;
    }


    private void OnApplicationQuit()
    {
        m_inventory.Container.Clear();
        m_inventory.weight= 0;
        //storages.Storages.Clear();

    }
}
