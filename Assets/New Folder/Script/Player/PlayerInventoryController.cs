using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInventoryController : MonoBehaviour
{
    public InventoryRecord inventory;

    public PlayerMoveMent pm;
    public PlayerControl pc;
    public PlayerCameraLook cameraLook;

    public Transform ItemContent;

    public ItemwheelController ItemwheelController;

    Dictionary<InventorySlot , GameObject> itemDisplayed = new Dictionary<InventorySlot, GameObject>();
    public List<GameObject> objToShow;
    public GameObject inventoryOBJ;
    public GameObject blurPanel;
    public bool inventoryOpen;
    public GameObject craft;
    public GameObject equip;
    public GameObject informationbar;

    [Header("Main Hand Item")]
    public Item pre_ItemSlot;
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



    }
    public void Craft()
    {
        craft.SetActive(true);
        informationbar.SetActive(false);
        equip.SetActive(false);



    }
    public void Information()
    {
       
        equip.SetActive(false);
        craft.SetActive(false);
        informationbar.SetActive(true);
     

    }
    void Update()
    {
        UpdateDisplay();        //背包物品狀態隨時更新

        slotIconShow();     //物品設定欄圖示設定

        numPointing();      //0322

        valueSync();




        if (Input.GetKeyDown(KeyCode.E))
        {

            inventoryOpen = !inventoryOpen;
            informationManger.CloseAll();

        }
        

        if (inventoryOpen && !ItemwheelController.weaponWheelSelected)
        {
            inventoryOBJ.SetActive(true);
            blurPanel.SetActive(true);
            pm.canMove = false;
            Cursor.visible = true;
            cameraLook.enabled = false;
            Cursor.lockState = CursorLockMode.Confined;
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

    IEnumerator watiFrame(UIItemManager uIItemManager , int i )
    {
        yield return new WaitForEndOfFrame();
        //Debug.Log(inventory.Container[i].pNum);

        if(inventory.Container.Count > 0) uIItemManager.pNum = inventory.Container[i].pNum;
        
        


    }
    public void IdiotInformationManger()
    {
        
        informationManger.SetItemType();

    }
}

[System.Serializable]
public class variables
{
    public int itemdoneness;
    public int itemhealth;

}
