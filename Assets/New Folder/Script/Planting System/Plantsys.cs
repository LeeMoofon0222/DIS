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
    public int p; // 當前植物生長階段索引
    private int period; // 植物生長所需時間
    private int id; // 植物ID
    private int step; // 植物生長階段數量
    public GameObject plant; // 當前植物物件
    private float planthealth; // 植物生命值
    public List<GameObject> plantlist; // 植物不同生長階段預製件列表
    private GameObject plantpoint; // 種植位置
    private GameObject player; // 玩家物件
    public int watering; // 玩家澆水狀態
    public bool hasplant = true; // 是否有植物
    public bool doplant = true; // 是否正在種植
    public Slider slider; // 生長進度條
    private bool test1 = true; // 用於控制澆水計時器

    public void Start()
    {
        slider.gameObject.SetActive(true); // 啟用生長進度條
    }

    public void Update()
    {
        player = GameObject.FindWithTag("Player"); // 獲取玩家物件
        watering = player.GetComponent<PlayerControl>().watering; // 獲取玩家澆水狀態

        // 如果玩家正在澆水且test1為true且正在種植
        if (player.GetComponent<PlayerControl>().watering == 1 && test1 && doplant)
        {
            Invoke("ReduceBar", 1); // 1秒後調用ReduceBar方法
            test1 = false; // 將test1設為false
        }

        // 如果正在種植且玩家正在澆水
        if (doplant && player.GetComponent<PlayerControl>().watering == 1)
        {
            Destroy(plant, period); // 在period時間後銷毀當前植物
            Invoke("PlantPlant", period); // 在period時間後調用PlantPlant方法
            doplant = false; // 將doplant設為false
        }

        // 如果有植物,將hasplant設為true,否則設為false
        if (plant != null)
        {
            hasplant = true;
        }
        else
        {
            hasplant = false;
            p = 0; // 重置生長階段索引
        }

        // 如果生長進度條為0,重置為period
        if (slider.value == 0)
        {
            slider.value = period;
        }

        // 如果生長階段索引等於最後一個階段,隱藏生長進度條
        if (p != 0 && p == plantlist.Count)
        {
            slider.gameObject.SetActive(false);
        }

        // 如果不在種植狀態,設置玩家澆水狀態為0
        if (!doplant)
        {
            player.GetComponent<PlayerControl>().watering = 0;
        }

        // 如果沒有植物,隱藏生長進度條並設置doplant為false
        if (!hasplant)
        {
            slider.gameObject.SetActive(false);
            doplant = false;
        }
    }

    // 開始種植
    public void DoPlanting(PlantObject thisseed)
    {
        doplant = false; // 設置不在種植狀態
        p = 0; // 重置生長階段索引
        period = thisseed.period; // 獲取植物生長所需時間
        plantlist = thisseed.plantstep; // 獲取植物生長階段預製件列表
        slider.maxValue = period; // 設置生長進度條最大值
        step = plantlist.Count; // 獲取生長階段數量
        id = thisseed.ID; // 獲取植物ID

        // 根據ID獲取種植位置
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

        // 實例化第一個生長階段
        plant = Instantiate(plantlist[p], plantpoint.transform.position, plantpoint.transform.rotation);
        slider.gameObject.SetActive(true); // 顯示生長進度條
        slider.maxValue = period; // 設置生長進度條最大值
        slider.value = period; // 設置生長進度條值為最大值
        plant.layer = 0; // 設置植物層級
        gameObject.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>().Play(); // 播放種植粒子特效
        planthealth = plant.AddComponent<ItemObject>().objectHealth; // 獲取植物生命值
        plant.GetComponent<Rigidbody>().isKinematic = true; // 設置植物為運動學剛體
        plant.GetComponent<ItemObject>().canPick = false; // 設置植物不可被撿起
        plant.GetComponent<ItemObject>().PickObj = false; // 設置植物不可被撿起
        p += 1; // 增加生長階段索引
        doplant = true; // 設置正在種植狀態
    }

    // 生成下一個生長階段
    public void PlantPlant()
    {
        plant = Instantiate(plantlist[p], plantpoint.transform.position, plantlist[p].transform.rotation); // 實例化下一個生長階段
        player.GetComponent<PlayerControl>().watering = 0; // 設置玩家澆水狀態為0
        slider.gameObject.SetActive(true); // 顯示生長進度條
        slider.maxValue = period; // 設置生長進度條最大值
        slider.value = period; // 設置生長進度條值為最大值
        test1 = true; // 設置test1為true
        gameObject.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>().Play(); // 播放種植粒子特效

        // 如果不是最後一個生長階段,設置正在種植狀態
        if (p < step - 1)
        {
            doplant = true;
        }
        p += 1; // 增加生長階段索引
    }

    // 減少生長進度條值
    public void ReduceBar()
    {
        slider.value -= 1; // 減少生長進度條值
        player.GetComponent<PlayerControl>().watering = 0; // 設置玩家澆水狀態為0

        // 如果生長進度條值不為0,在1秒後再次調用ReduceBar
        if (slider.value != 0)
        {
            Invoke("ReduceBar", 1);
        }
    }
}