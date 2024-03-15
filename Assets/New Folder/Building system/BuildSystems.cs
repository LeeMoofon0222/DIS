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

    public int wood_amount = 0; // 玩家擁有的木頭數量
    public int stone_amount = 0; // 玩家擁有的石頭數量

    [SerializeField]
    public List<GameObject> Parts_pb;
    [SerializeField]
    public List<GameObject> buildingParts;
    [SerializeField]
    public List<GameObject> still_pre;
    [SerializeField]
    public List<GameObject> all_build;

    public InventoryRecord ir; // 存取玩家背包的資訊

    int T = 0;
    [SerializeField]
    public List<int> the_wood_Cost;
    [SerializeField]
    public List<int> the_stone_Cost;

    public float ry; // 旋轉角度
    public bool BuildFoundation = true; // 是否正在建造地基

    public List<int> wood_cost; // 每種建築所需的木頭數量
    public List<int> stone_cost; // 每種建築所需的石頭數量

    RaycastHit hit;
    float nowItem; // 目前選擇的建築索引
    float t;

    bool isSpawned; // 是否已生成預製地基
    bool partisSpawned; // 是否已生成預製建築

    GameObject preFoundation; // 預製地基物件
    public GameObject preBuildParts; // 預製建築物件

    public void Start()
    {
        // 初始化木頭和石頭數量
        stone_amount = ir.ItemCount(10001, stone_amount);
        wood_amount = ir.ItemCount(10002, wood_amount);
    }

    void Update()
    {
        // 更新木頭和石頭數量
        stone_amount = ir.ItemCount(10001, stone_amount);
        wood_amount = ir.ItemCount(10002, wood_amount);

        // 朝前方射出射線，判斷是否為地基或可建造區域
        bool checkhit = Physics.Raycast(cam.position, cam.forward, out RaycastHit buildhit, 7f, buildMask);

        // 使用滾輪切換建築選項
        float mw = Input.GetAxis("Mouse ScrollWheel");
        t += mw * 10f;
        nowItem += mw * 10f;
        if (t > 1f)
        {
            t = 0;
        }
        if (t < 0)
        {
            t = -t;
        }
        T = Mathf.RoundToInt(t);

        // 按下B鍵切換為建造地基模式
        if (Input.GetKeyUp(KeyCode.B))
        {
            nowItem = 0;
            nowItem = Mathf.Repeat(nowItem, all_build.Count);
            BuildFoundation = !BuildFoundation;
            isSpawned = false;
            stone_amount = ir.ItemCount(10001, stone_amount);
            wood_amount = ir.ItemCount(10002, wood_amount);
            if (BuildFoundation == false)
            {
                Destroy(GameObject.FindGameObjectWithTag("prebuildFoundation"));
            }
        }

        // 處理建築選項切換
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

        // 滾動滾輪時移除預製建築
        if (mw != 0)
        {
            GameObject _gameObject = GameObject.FindGameObjectWithTag("prebuildParts");
            Destroy(_gameObject);
            partisSpawned = false;
            isSpawned = false;
        }

        // 如果射線打中地基或可建造區域，進行建造流程
        if (checkhit)
        {
            int foundation = LayerMask.NameToLayer("foundation");
            int canBuildOn = LayerMask.NameToLayer("canBuildOn");

            // 按下X鍵拆除建築
            if (Input.GetKeyUp(KeyCode.X) && (buildhit.transform.gameObject.layer == canBuildOn || buildhit.transform.gameObject.layer == foundation) && buildhit.transform.gameObject.name[0] != 'D')
            {
                // 拆除地基
                if (buildhit.transform.gameObject.layer == foundation)
                {
                    if (buildhit.transform.gameObject.tag == "Foundation" && buildhit.transform.gameObject.name[0] != 'D')
                    {
                        ir.AddItem(ir.IDtoItem(10001), 3, 0, 0); // 返還3個石頭
                        Destroy(buildhit.transform.gameObject);
                    }
                    else
                    {
                        Destroy(buildhit.transform.gameObject);
                    }
                }
                // 拆除其他建築
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
                            ir.AddItem(ir.IDtoItem(10002), the_wood_Cost[i], 0, 0); // 返還所需木頭
                            ir.AddItem(ir.IDtoItem(10001), the_stone_Cost[i], 0, 0); // 返還所需石頭
                        }
                    }
                }
                // 更新木頭和石頭數量
                stone_amount = ir.ItemCount(10001, stone_amount);
                wood_amount = ir.ItemCount(10002, wood_amount);
            }
        }

        // 如果射線打中且不在建造地基模式
        if (checkhit && !BuildFoundation)
        {
            whatcanibuild(stone_amount, wood_amount); // 檢查可建造的建築選項
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
                    building_materials.text = "石頭: " + stone_cost[setItem] + " 木頭: " + wood_cost[setItem]; // 顯示所需材料
                    Instantiate(Parts_pb[setItem]); // 生成預製建築
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
                BuildOnFoundation(preBuildParts.transform, buildingParts[setItem], buildhit, setItem); // 建造在地基上
            }
        }
        // 如果射線沒打中且不在建造地基模式，移除預製建築
        else if (!checkhit && !BuildFoundation)
        {
            GameObject _gameObject = GameObject.FindGameObjectWithTag("prebuildParts");
            Destroy(_gameObject);
            partisSpawned = false;
        }

        // 如果在建造地基模式
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
                    building_materials.text = "石頭: 3"; // 顯示所需材料
                    Instantiate(preFoundationObj[0]); // 生成預製地基
                    isSpawned = true;
                    partisSpawned = false;
                }
                if (T == 1)
                {
                    building_materials.text = "無須耗費材料";
                    Instantiate(preFoundationObj[1]);
                    isSpawned = true;
                    partisSpawned = false;
                }
            }
        }
        preFoundation = GameObject.FindGameObjectWithTag("prebuildFoundation");

        // 如果不在建造地基模式且有預製建築存在，檢查是否為有效建築選項
        if (!BuildFoundation && GameObject.FindGameObjectWithTag("prebuildParts") != null)
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

        // 處理地基生成位置和判斷是否可以生成
        if (Physics.Raycast(cam.position, cam.forward, out hit, 14f) && BuildFoundation)
        {
            preFoundation.transform.position = new Vector3(
                Mathf.RoundToInt(hit.point.x) != 0 ? Mathf.RoundToInt(hit.point.x / 12f) * 12f : 12f,
                Mathf.RoundToInt(hit.point.y) != 0 ? Mathf.RoundToInt(hit.point.y / 1f) * 1f : 0f,
                Mathf.RoundToInt(hit.point.z) != 0 ? Mathf.RoundToInt(hit.point.z / 12f) * 12f : 12f);
            Vector3 world_pos = preFoundation.transform.position;
            bool testBox = Physics.CheckBox(new Vector3(preFoundation.transform.position.x, preFoundation.transform.position.y + 6f, preFoundation.transform.position.z),
              new Vector3(preFoundation.transform.localScale.x / 2 - 0.01f, 6.5f, preFoundation.transform.localScale.z / 2 - 0.01f), Quaternion.identity, mask);

            if (testBox)
            {
                preFoundation.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.6f); // 顯示為紅色，代表不可建造
            }
            else // 可以建造
            {
                preFoundation.GetComponent<Renderer>().material.color = new Color(0, 1, 1, 0.6f); // 顯示為綠色
                if (Input.GetMouseButtonDown(0))
                {
                    if (ir.FindItem(10001, 3) && T == 0) // 檢查是否有足夠石頭
                    {
                        ir.DecreesItem(10001, 3); // 扣除3個石頭
                        Instantiate(foundation[T], preFoundation.transform.position, preFoundation.transform.rotation); // 生成地基
                    }
                    else if (T == 1) // 無須耗材
                    {
                        Instantiate(foundation[T], preFoundation.transform.position, preFoundation.transform.rotation);
                    }
                }
            }
        }
    }

    // 檢查可建造的建築選項
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

    // 在地基上建造建築
    void BuildOnFoundation(Transform pb, GameObject buildObject, RaycastHit buildhit, int setitem)
    {
        pb.position = cal_spawnPos(buildhit); // 計算生成位置
        pb.eulerAngles = new Vector3(0f, Mathf.RoundToInt(transform.eulerAngles.y) != 0 ? Mathf.RoundToInt(transform.eulerAngles.y / 90f) * 90f : 0, 0f); // 設定旋轉角度

        if (Input.GetKeyDown(KeyCode.R))
        {
            ry += 90f; // 按R鍵旋轉90度
        }
        pb.eulerAngles = new Vector3(0f, ry, 0);

        if (Input.GetMouseButtonDown(0))
        {
            // 在地基上生成建築
            Instantiate(buildObject, pb.position, pb.rotation); // 生成建築

            // 扣除所需木頭和石頭
            ir.DecreesItem(10002, wood_cost[setitem]); // 扣除所需木頭
            ir.DecreesItem(10001, stone_cost[setitem]); // 扣除所需石頭

            // 更新可建造的建築選項
            whatcanibuild(stone_amount, wood_amount);

            // 檢查生成的建築是否在建築物件列表中，若不在則重置選擇為第一個建築物件
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
                else if (i == buildingParts.Count - 1)
                {
                    nowItem = 0;
                    setitem = 0;
                }
            }
        }
    }

    // 計算生成位置
    public Vector3 cal_spawnPos(RaycastHit buildhit)
    {
        if (buildhit.transform.tag == "Foundation" || buildhit.transform.tag == "canBuildOn")
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
        // 重置狀態並啟用建造界面
        isSpawned = false;
        building_ui.SetActive(true);
    }

    // 禁用時的處理
    private void OnDisable()
    {
        // 關閉建造界面並重置狀態
        building_ui.SetActive(false);
        BuildFoundation = true;
        Parts_pb.Clear();
        buildingParts.Clear();
        nowItem = 0;

        // 刪除預製的地基和建築物件
        GameObject fundationdestroy = GameObject.FindGameObjectWithTag("prebuildFoundation");
        GameObject partsdestroy = GameObject.FindGameObjectWithTag("prebuildParts");
        Destroy(fundationdestroy);
        Destroy(partsdestroy);
    }
}
