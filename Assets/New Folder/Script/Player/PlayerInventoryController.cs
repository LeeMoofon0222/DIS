using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;


public class PlayerInventoryController : MonoBehaviour
{
    public InventoryRecord inventory;

    public PlayerMoveMent pm;
    public PlayerControl pc;
    public PlayerCameraLook cameraLook;

    public Transform ItemContent;
    

    public ItemwheelController ItemwheelController;

    Dictionary<InventorySlot , GameObject> itemDisplayed = new Dictionary<InventorySlot, GameObject>();
    Dictionary<StorageSlot , GameObject> storageitemDisplayed= new Dictionary<StorageSlot , GameObject>();
    public List<GameObject> objToShow;
    public GameObject inventoryOBJ;
    public GameObject blurPanel;
    public bool inventoryOpen;
    public GameObject craft;
    public GameObject equip;
    public GameObject informationbar;

    public AudioSource uiSources;
    public AudioClip selectClip;

    [Header("Storage")]
    public bool isStorage;
    public GameObject addItemPanel;
    public GameObject storagePanel;
    //public GameObject s_CanclePanel;
    public int storagemode;     //0 upload 1 download to/form storage 
    public StorageRecord storageRecord;
    public int storageNum;
    public Transform StorageItemContent;
    public GameObject storageslotObject;
    public List<GameObject> s_objToShow;
    bool stopupload;
    int storagepos;
    public bool panelCanOpen;

    [Header("Main Hand Item")]
    public Item pre_ItemSlot;
    public int pre_amount;
    public int pre_itemhealth;
    public int pre_itemdoneness;

    public List<Item> itemsOnHand;
    public List<variables> itemProperties;
    public Transform mainHandHolder;
    public List<Image> slotIcon;
    public Sprite none;
    

    public List<int> i_pnum;     //0322
    public int preset_pnum  = -1;

    int removePoint = 0;
    public InformationManger informationManger;
    //public GameObject Informationpage;

   
    

    // Update is called once per frame
    void Awake()
    {
        craft.SetActive(false);
        equip.SetActive(false);
        informationbar.SetActive(false);
    }

    public void Equipment()
    {

        craft.SetActive(false);
        informationbar.SetActive(false);
        equip.SetActive(true);
        storagePanel.SetActive(false);


        panelCanOpen = false;
    }
    public void Craft()
    {
        craft.SetActive(true);
        informationbar.SetActive(false);
        equip.SetActive(false);
        storagePanel.SetActive(false);

        panelCanOpen = false;

    }
    public void Information()
    {
       
        equip.SetActive(false);
        craft.SetActive(false);
        storagePanel.SetActive(false);
        informationbar.SetActive(true);

        panelCanOpen = false;
    }

    public void StorageOpen()
    {
        //storagePanel.SetActive(true);
        panelCanOpen = true;
        craft.SetActive(false);
        informationbar.SetActive(false);
        equip.SetActive(false);
    }
    void Update()
    {
        UpdateDisplay();        //背包物品狀態隨時更新

        slotIconShow();     //物品設定欄圖示設定

        numPointing();      //0322

        valueSync();

        /*if(!stopupload)
            UpdateStorage();
        */

        if (Input.GetKeyDown(KeyCode.E))
        {

            inventoryOpen = !inventoryOpen;
            informationManger.CloseAll();
            if(isStorage) UpdateStorage();

        }
        

        if (inventoryOpen && !ItemwheelController.weaponWheelSelected)
        {
            inventoryOBJ.SetActive(true);
            blurPanel.SetActive(true);
            pm.canMove = false;
            Cursor.visible = true;
            cameraLook.enabled = false;
            Cursor.lockState = CursorLockMode.Confined;

            if(isStorage && panelCanOpen)
                storagePanel.SetActive(true);
            
        }
        else
        {
            inventoryOBJ.SetActive(false);
            blurPanel.SetActive(false);
            pm.canMove = true;
            cameraLook.enabled = true;
            //Cursor.lockState = CursorLockMode.Locked;
            craft.SetActive(false);
            equip.SetActive(false);
            informationbar.SetActive(false);
            pre_ItemSlot = null;

            storagePanel.SetActive(false);
        }

    }


    public void UpdateDisplay()
    {
        for(int i = 0; i < inventory.Container.Count; i++)
        {
            if ((!itemDisplayed.ContainsKey(inventory.Container[i])) && inventory.Container[i].amount > 0)
            {
                GameObject obj = Instantiate(inventory.Container[i].item.UIObject, ItemContent);
                objToShow.Add(obj);

                var _itemName = obj.transform.Find("ItemName").GetComponent<Text>();
                var _itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
                

                _itemName.text = inventory.Container[i].item.itemName;
                _itemIcon.sprite = inventory.Container[i].item.itemIcon;

                if(obj.TryGetComponent(out UIItemManager uiItemManager))
                {
                    uiItemManager.item = inventory.Container[i].item;

                    StartCoroutine(watiFrame(uiItemManager, i));

                }

                itemDisplayed.Add(inventory.Container[i], obj);
                
            }

            if(inventory.Container[i].amount <= 0 || 
              (inventory.Container[i].item_Health <= 0 && (inventory.Container[i].item.type == ItemType.Tool || inventory.Container[i].item.type == ItemType.Food)))    //0323
            {   
                for(int j = 0; j < itemsOnHand.Count; j++)
                {
                    if(itemsOnHand[j] == inventory.Container[i].item )      
                    {
                        itemsOnHand[j] = null;
                    }
                }

                removePoint = i+1;      //0322


                Destroy(objToShow[i]);
                objToShow.RemoveAt(i);

                itemDisplayed.Remove(inventory.Container[i]);
                inventory.Container.Remove(inventory.Container[i]);

                
            }
        }
    }

    public void UpdateStorage()
    {
        storageitemDisplayed.Clear();
        foreach (var obj in s_objToShow) Destroy(obj);

        for (int i = 0; i < 25; i++)
        {
            if ((!storageitemDisplayed.ContainsKey(storageRecord.Storages[storageNum].Container[i])))
            {
                GameObject obj = Instantiate(storageslotObject, StorageItemContent);
                s_objToShow.Add(obj);

                //var _itemName = obj.transform.Find("ItemName").GetComponent<Text>();
                var _itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

                if(storageRecord.Storages[storageNum].Container[i].item != null)
                    _itemIcon.sprite = storageRecord.Storages[storageNum].Container[i].item.itemIcon;

                if (obj.TryGetComponent(out UIItemManager uiItemManager))
                {
                    uiItemManager.item = storageRecord.Storages[storageNum].Container[i].item;
                    uiItemManager.amount = storageRecord.Storages[storageNum].Container[i].amount;
                    uiItemManager.doneness = storageRecord.Storages[storageNum].Container[i].doneness;
                    uiItemManager.health = storageRecord.Storages[storageNum].Container[i].item_Health;

                    uiItemManager.pos = i;

                    //StartCoroutine(watiFrame(uiItemManager, i));

                }

                storageitemDisplayed.Add(storageRecord.Storages[storageNum].Container[i], obj);

            }
        }

    }

    public void numPointing()       //0322
    {
        for(int i = 0; i < inventory.Container.Count; i++)
        {
            inventory.Container[i].pNum = i + 1;
        }

        for(int j = 0; j < i_pnum.Count; j++)
        {
            if (i_pnum[j] >= removePoint && removePoint != 0)
            {
                i_pnum[j]-- ;
            }
        }

        removePoint = 0;

    }


    public void InventoryControl(int slotNum)       //背包設定
    {   
        if(pre_ItemSlot != null)
        {
            itemsOnHand[slotNum] = pre_ItemSlot;        //複製物品你選取的 
            pre_ItemSlot = null;

            i_pnum[slotNum] = preset_pnum;      //0322
            preset_pnum = -1;

        }
        
        if(pc.setItem == slotNum)       
        {
            pc.RepairItemOnHand();       //重製手上物品
        }

    }

    void slotIconShow()
    {
        for (int i = 0; i < itemsOnHand.Count; i++)
        {

            if (itemsOnHand[i] == null)     //如果物品欄當前格是空的 0322
            {
                slotIcon[i].enabled = false;
                slotIcon[i].sprite = none;
            }
            else        //物品欄當前格有東西
            {
                slotIcon[i].enabled = true;
                slotIcon[i].sprite = itemsOnHand[i].itemIcon;
            }

        }

    }

    void valueSync()        //數值同步
    {
        for(int i =0; i < itemsOnHand.Count; i++)
        {
            if (itemsOnHand[i] != null)
            {
                itemProperties[i].itemhealth = inventory.GetItemhealth(itemsOnHand[i], i_pnum[i]);
                itemProperties[i].itemdoneness = inventory.GetBBQStep(itemsOnHand[i], i_pnum[i]);
            }
            else if (itemsOnHand[i] == null)
            {
                itemProperties[i].itemhealth = 0;
                itemProperties[i].itemdoneness = 0;
            }
            
        }
    }
    
    public void StoreMainhandItem(Item _item , int _pnum)       //0322
    {
        pre_ItemSlot = _item;
        preset_pnum = _pnum;        
    }
    public void SaveSelected(Item _item, int _amount, int doneness, int itemhealth, int _storagepos)    //取出
    {
        pre_ItemSlot= _item;
        pre_amount = _amount;
        pre_itemdoneness= doneness;
        pre_itemhealth = itemhealth;

        storagepos = _storagepos;

        addItemPanel.SetActive(true);
    }

    public void StoreItem(int _pos)
    {
        if(storagemode == 0)
        {
            if (storageRecord.Storages[storageNum].Container[_pos].item == null)
            {
                if (pre_ItemSlot != null)
                {
                    var itm = storageRecord.Storages[storageNum].Container[_pos];
                    itm.item = pre_ItemSlot;
                    itm.amount = 1;
                    itm.doneness = inventory.GetBBQStep(pre_ItemSlot, preset_pnum);
                    itm.item_Health = inventory.GetItemhealth(pre_ItemSlot, preset_pnum);

                    inventory.DecreesItem(pre_ItemSlot, 1, preset_pnum);

                    storageitemDisplayed.Clear();
                    foreach (var obj in s_objToShow) Destroy(obj);
                }
            }
            Cancle();
        }
        UpdateStorage();

    }


    public void AddItem()       //加入背包
    {
        if(pre_ItemSlot != null)
        {
            inventory.AddItem(pre_ItemSlot, pre_amount, pre_itemhealth, pre_itemdoneness);
            addItemPanel.SetActive(false);

            storageRecord.Storages[storageNum].Container[storagepos].item = pre_ItemSlot = null;
            storageRecord.Storages[storageNum].Container[storagepos].amount = pre_amount = 0;
            storageRecord.Storages[storageNum].Container[storagepos].doneness = pre_itemdoneness = 0;
            storageRecord.Storages[storageNum].Container[storagepos].item_Health = pre_itemhealth = 0;

            storagepos = 0;

            storageitemDisplayed.Clear();
            foreach (var obj in s_objToShow) Destroy(obj);
        }
        Cancle();
        UpdateStorage();

    }

    public void Cancle()        //取消選取
    {
        pre_ItemSlot = null;
        pre_amount = 0;
        pre_itemdoneness = 0;
        pre_itemhealth = 0;

        storagepos = 0;
        addItemPanel.SetActive(false);
        UpdateStorage();

    }

    public void IdiotInformationManger()
    {
        informationManger.SetItemType();

    }

    IEnumerator watiFrame(UIItemManager uIItemManager, int i)
    {
        yield return new WaitForEndOfFrame();
        //Debug.Log(inventory.Container[i].pNum);

        if (inventory.Container.Count > 0) uIItemManager.pNum = inventory.Container[i].pNum;

    }

}

[System.Serializable]
public class variables
{
    public int itemdoneness;
    public int itemhealth;

}


