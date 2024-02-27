using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildSystems : MonoBehaviour
{
    [SerializeField]
    LayerMask mask;

    [SerializeField]
    Transform cam;
    [SerializeField]
    public List<GameObject> preFoundationObj;
    [SerializeField]
    public List<GameObject> foundation;
    [SerializeField]
    public TMP_Text building_materials;
    public GameObject building_ui;
    public LayerMask buildMask;

    public int wood_amount = 0;

    public int stone_amount = 0;

    [SerializeField]
    public List<GameObject> Parts_pb;
    [SerializeField]
    public List<GameObject> buildingParts;
    [SerializeField]
    public List<GameObject> still_pre;
    [SerializeField]
    public List<GameObject> all_build;
    
    public InventoryRecord ir;


    int T = 0;
    [SerializeField]
    public List<int> the_wood_Cost;

    [SerializeField]
    public List<int> the_stone_Cost;
    //public LayerMask notForBuild;

    public float ry;
    public bool BuildFoundation = true;

    public List<int> wood_cost;
    public List<int> stone_cost;

    RaycastHit hit;
    float nowItem;
    float t;
    public object Keycode { get; private set; }

    bool isSpawned;
    bool partisSpawned;

    GameObject preFoundation;
    public GameObject preBuildParts;

    public void Start()
    {
        stone_amount = ir.ItemCount(10001, stone_amount);
        wood_amount = ir.ItemCount(10002, wood_amount);
        
    }
    void Update()
    {
        stone_amount = ir.ItemCount(10001, stone_amount);
        wood_amount = ir.ItemCount(10002, wood_amount);
        bool checkhit = Physics.Raycast(cam.position, cam.forward, out RaycastHit buildhit, 7f, buildMask);
        float mw = Input.GetAxis("Mouse ScrollWheel");
        t += mw * 10f; 
        nowItem += mw * 10f;
        if(t > 1f)
        {
            t = 0;
        }
        if (t < 0)
        {
            t = -t;
        }
        T = Mathf.RoundToInt(t);
        if (Input.GetKeyUp(KeyCode.B)) 
        {
            nowItem = 0;
            nowItem = Mathf.Repeat(nowItem, all_build.Count);
            BuildFoundation = !BuildFoundation;
            isSpawned = false;
            stone_amount = ir.ItemCount(10001, stone_amount);
            wood_amount = ir.ItemCount(10002, wood_amount);
            if(BuildFoundation==false) 
            {
                Destroy(GameObject.FindGameObjectWithTag("prebuildFoundation"));
            }
        }
        if (nowItem == Parts_pb.Count)
        {
            nowItem = 0;
        }
        if (nowItem < 0)
        {
            if (nowItem == -Parts_pb.Count)
            {
                nowItem = 0;
            }
        }
        int setItem = Mathf.RoundToInt(nowItem);
        if (setItem < 0) 
        {
            setItem = -setItem;
        }

        if (mw != 0)
        {
            GameObject _gameObject = GameObject.FindGameObjectWithTag("prebuildParts");
            Destroy(_gameObject);
            partisSpawned = false;
            isSpawned = false;
        }

        if (checkhit)
        {
            int foundation = LayerMask.NameToLayer("foundation");
            int canBuildOn = LayerMask.NameToLayer("canBuildOn");

            //拆除
            if (Input.GetKeyUp(KeyCode.X) && (buildhit.transform.gameObject.layer == canBuildOn || buildhit.transform.gameObject.layer == foundation) && buildhit.transform.gameObject.name[0]!='D')
            {

                if (buildhit.transform.gameObject.layer == foundation)
                {
                    if (buildhit.transform.gameObject.tag == "Foundation" && buildhit.transform.gameObject.name[0] != 'D')
                    {
                        ir.AddItem(ir.IDtoItem(10001), 3, 0, 0);
                        Destroy(buildhit.transform.gameObject);
                    }
                    else
                    {
                        Destroy(buildhit.transform.gameObject);
                    }
                }
                else
                {
                    for (int i = 0; i < all_build.Count; i++)
                    {
                        string fname = buildhit.transform.gameObject.name;
                        int a = fname.IndexOf("(");
                        if (a == -1)
                        {
                            a = fname.Length;
                        }
                        fname = fname.Substring(0, a);
                        if (fname == all_build[i].name)
                        {
                            Destroy(buildhit.transform.gameObject);
                            ir.AddItem(ir.IDtoItem(10002), the_wood_Cost[i], 0, 0);
                            ir.AddItem(ir.IDtoItem(10001), the_stone_Cost[i], 0, 0);
                        }
                    }
                }
                stone_amount = ir.ItemCount(10001, stone_amount);
                wood_amount = ir.ItemCount(10002, wood_amount);
            }
        }
        if (checkhit && !BuildFoundation)
        {
            whatcanibuild(stone_amount, wood_amount);
            if (Parts_pb.Count > 0)
            {
                if (!partisSpawned)
                {
                    if (setItem < 0)
                    {
                        setItem = -setItem;
                    }
                    if (setItem >= Parts_pb.Count)
                    {
                        setItem = 0;
                    }
                    building_materials.text = "石頭: " + stone_cost[setItem] + " 木頭: " + wood_cost[setItem];
                    Instantiate(Parts_pb[setItem]);
                    /*
                    try
                    {
                        Instantiate(Parts_pb[setItem]);
                    }
                    catch (Exception)
                    {
                        setItem = 0;
                        Instantiate(Parts_pb[setItem]);
                    }*/
                }
                partisSpawned = true;
                GameObject preBuildParts = GameObject.FindGameObjectWithTag("prebuildParts");
                if (setItem < 0)
                {
                    setItem = -setItem;
                }
                if (setItem >= Parts_pb.Count)
                {
                    setItem = 0;
                }
                whatcanibuild(stone_amount, wood_amount);
                /*
                print(preBuildParts.transform);
                print(setItem);
                print(buildhit);*/
                BuildOnFoundation(preBuildParts.transform, buildingParts[setItem], buildhit, setItem);//偵測建物
            }
        }
        else if (!checkhit && !BuildFoundation)
        {
            GameObject _gameObject = GameObject.FindGameObjectWithTag("prebuildParts");
            Destroy(_gameObject);
            partisSpawned = false;
        }
        //Debug.Log(isSpawned);

        if (BuildFoundation)
        {
            GameObject preBuildParts = GameObject.FindGameObjectWithTag("prebuildParts");
            Destroy(preBuildParts);

            if (!isSpawned)
            {
                GameObject preF = GameObject.FindGameObjectWithTag("prebuildFoundation");
                Destroy(preF);
                if (T == 0)
                {
                    //print("spawn");
                    building_materials.text = "石頭: 3";
                    Instantiate(preFoundationObj[0]);
                    isSpawned = true;
                    partisSpawned = false;
                }
                if (T == 1)
                {
                    //print("spawn");
                    building_materials.text = "無須耗費材料";
                    Instantiate(preFoundationObj[1]);
                    isSpawned = true;
                    partisSpawned = false;
                }
            }
        }
        preFoundation = GameObject.FindGameObjectWithTag("prebuildFoundation");


        if (!BuildFoundation && GameObject.FindGameObjectWithTag("prebuildParts")!=null)
        {
            GameObject Tg = GameObject.FindGameObjectWithTag("prebuildParts");
            string Tname = Tg.name;
            int b = Tname.IndexOf("(");
            if (b == -1)
            {
                b = Tname.Length;
            }
            Tname = Tname.Substring(0, b);
            for (int i = 0; i < Parts_pb.Count; i++)
            {
                if (Tname == Parts_pb[i].name)
                {
                    break;
                }
                else if (i == Parts_pb.Count - 1)
                {
                    Destroy(GameObject.FindGameObjectWithTag("prebuildParts"));
                    partisSpawned = false;
                }
            }
            Destroy(preFoundation);
            isSpawned = false;
        }

        //=================================================================地基=======================================================================================================

        if (Physics.Raycast(cam.position, cam.forward, out hit, 14f) && BuildFoundation )//射線的發射點，射線的方向，射線的點，射線的最大距離(t是判定讓你能不能建地基)
        {     
            preFoundation.transform.position = new Vector3(
                Mathf.RoundToInt(hit.point.x) != 0 ? Mathf.RoundToInt(hit.point.x / 12f) * 12f : 12f,
                Mathf.RoundToInt(hit.point.y) != 0 ? Mathf.RoundToInt(hit.point.y / 1f) * 1f : 0f,
                Mathf.RoundToInt(hit.point.z) != 0 ? Mathf.RoundToInt(hit.point.z / 12f) * 12f : 12f);
            Vector3 world_pos = preFoundation.transform.position;
            bool testBox = Physics.CheckBox(new Vector3(preFoundation.transform.position.x, preFoundation.transform.position.y + 6f, preFoundation.transform.position.z),
              new Vector3(preFoundation.transform.localScale.x / 2 - 0.01f, 6.5f, preFoundation.transform.localScale.z / 2 - 0.01f), Quaternion.identity, mask);
            //bool testBox = Physics.CheckBox(new Vector3(preFoundation.transform.position.x + 6.5f, preFoundation.transform.position.y + 6.5f, preFoundation.transform.position.z + 6.5f),
                //new Vector3(preFoundation.transform.localScale.x / 2 - 0.01f, 6f, preFoundation.transform.localScale.z / 2 - 0.01f), Quaternion.identity, mask);

            if (testBox)
            {
                preFoundation.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.6f);
            }
            else //Can Build
            {
                preFoundation.GetComponent<Renderer>().material.color = new Color(0, 1, 1, 0.6f);
                if (Input.GetMouseButtonDown(0))
                {
                    //print(T);
                    if (ir.FindItem(10001 , 3) && T == 0)
                    {
                        ir.DecreesItem(10001, 3);
                        
                        Instantiate(foundation[T], preFoundation.transform.position, preFoundation.transform.rotation);
                    }
                    else if (T == 1)
                    {
                        
                        Instantiate(foundation[T], preFoundation.transform.position, preFoundation.transform.rotation);
                    }
                }
            }
        }
        //=================================================================地基====================================================================================================

    }

    public void whatcanibuild(int stone_amount, int wood_amount)
    {
        Parts_pb.Clear();
        buildingParts.Clear();
        wood_cost.Clear();
        stone_cost.Clear();
        for (int i = 0; i < all_build.Count; i++)
        {
            if (wood_amount >= the_wood_Cost[i] && stone_amount >= the_stone_Cost[i])
            {
                Parts_pb.Add(still_pre[i]);
                buildingParts.Add(all_build[i]);
                wood_cost.Add(the_wood_Cost[i]);
                stone_cost.Add(the_stone_Cost[i]);
            }
        }
    }

    void BuildOnFoundation(Transform pb, GameObject buildObject , RaycastHit buildhit , int setitem)
    {
        pb.position = cal_spawnPos(buildhit);
        pb.eulerAngles = new Vector3(0f,  Mathf.RoundToInt(transform.eulerAngles.y) != 0 ? Mathf.RoundToInt(transform.eulerAngles.y / 90f) * 90f : 0 , 0f);

        if (Input.GetKeyDown(KeyCode.R))
        { 
            ry += 90f; 
        }
        pb.eulerAngles = new Vector3(0f, ry, 0);
        
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(buildObject, pb.position, pb.rotation);

            ir.DecreesItem(10002, wood_cost[setitem]);
            ir.DecreesItem(10001, stone_cost[setitem]);

            whatcanibuild(stone_amount, wood_amount);
            for (int i = 0; i < buildingParts.Count; i++)
            {
                string bname = buildObject.name;
                int a = bname.IndexOf("(");
                if (a == -1)
                {
                    a = bname.Length;
                }
                bname = bname.Substring(0, a);
                if (bname == all_build[i].name)
                {
                    break;
                }
                else if(i == buildingParts.Count - 1)
                {
                    nowItem = 0;
                    setitem = 0;
                }
            }
        }
    }

    public Vector3 cal_spawnPos(RaycastHit buildhit)
    {   
        if(buildhit.transform.tag == "Foundation" || buildhit.transform.tag == "canBuildOn")
        {
            Vector3 min_foundationRange = new Vector3(buildhit.transform.position.x - 6f,
                                          buildhit.transform.position.y,
                                          buildhit.transform.position.z - 6f);

            float re_X = 0, re_Z = 0;

            for (int i = 1; i <= 4; i++)
            {
                if (buildhit.point.x <= min_foundationRange.x + 3f * i && buildhit.point.x > min_foundationRange.x + 3f * (i - 1))
                {
                    re_X = min_foundationRange.x + 1.5f + 3f * (i - 1);
                }
                if (buildhit.point.z <= min_foundationRange.z + 3f * i && buildhit.point.z > min_foundationRange.z + 3f * (i - 1))
                {
                    re_Z = min_foundationRange.z + 1.5f + 3 * (i - 1);
                }
            }
            return new Vector3(re_X, buildhit.transform.position.y + 0.55f, re_Z);
        }
        else
        {
            return buildhit.transform.position;
        }
    }
    private void OnEnable()
    {
        isSpawned = false;
        building_ui.SetActive(true);
    }

    private void OnDisable()
    {
        building_ui.SetActive(false);
        BuildFoundation = true;
        Parts_pb.Clear();
        buildingParts.Clear();
        nowItem = 0;
        GameObject fundationdestroy = GameObject.FindGameObjectWithTag("prebuildFoundation");
        GameObject partsdestroy = GameObject.FindGameObjectWithTag("prebuildParts");
        Destroy(fundationdestroy);
        Destroy(partsdestroy);
    }
}
