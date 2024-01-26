using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InformationManger : MonoBehaviour
{
    public Text Title;
    public GameObject Food;
    public GameObject Loot;
    public GameObject Tool;
    public GameObject Main;
    public PlayerInventoryController PlayerInventoryController;
    public InventoryRecord inventory;
    public Item item;
    public Text ItemInformation;
    private ToolObject toolObject;
    private FoodObject FoodObject;
    public Text itemWights;
    public Text itemCOunt;
    public Text Toolhealth;
    public Text ToolAtk;
    public Text ToolDEf;
    public Text food;
    public Text foodhealing;
    private PlantObject plant;
    public GameObject Plant;
    public Text period;
    int ammount;
    int Toolhealthing;
    public GameObject Name;
    // Start is called before the first frame update
    void Awake()
    {
        CloseAll();
    }
    public void CloseAll()
    {
        Name.SetActive(false);
        Main.SetActive(false);
        Food.SetActive(false);
        Loot.SetActive(false);
        Tool.SetActive(false);
        Plant.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        item = PlayerInventoryController.pre_ItemSlot;

    }

    public void MainInformation()
    {
        GetItemInformation(PlayerInventoryController.pre_ItemSlot);
        CloseAll();
        Main.SetActive(true);
        itemWights.text = "重量:" + PlayerInventoryController.pre_ItemSlot.itemWeights.ToString();

        itemCOunt.text = "數量:" + ammount.ToString();
        ItemInformation.text = PlayerInventoryController.pre_ItemSlot.desciption;
        Title.text = PlayerInventoryController.pre_ItemSlot.itemName;
        Name.SetActive(true);

    }
    public void SetItemType()
    {

        if (PlayerInventoryController.pre_ItemSlot.type == ItemType.Loot)
        {
            MainInformation();
            Loot.SetActive(true);
           
        }
        else if (PlayerInventoryController.pre_ItemSlot.type == ItemType.Tool)
        {

            MainInformation();
            toolObject = (ToolObject)PlayerInventoryController.pre_ItemSlot;
            Tool.SetActive(true);

            ToolAtk.text = "傷害:" + toolObject.ToolATK.ToString();
            ToolDEf.text = "防禦:" + toolObject.ToolDEF.ToString();
            Toolhealth.text = "耐久:" + Toolhealthing.ToString();


        }
        else if(PlayerInventoryController.pre_ItemSlot.type == ItemType.Food)
        {
            MainInformation();
            FoodObject = (FoodObject)PlayerInventoryController.pre_ItemSlot;
            Food.SetActive(true);

            food.text = "飽食度:" + FoodObject.satiety.ToString();
            foodhealing.text = "回血量:" + FoodObject.healing.ToString();

        }
        else if(PlayerInventoryController.pre_ItemSlot.type == ItemType.Plant)
        {
            MainInformation();
            plant = (PlantObject)PlayerInventoryController.pre_ItemSlot;
            Plant.SetActive(true);
          
            period.text = "成長度:" + plant.period.ToString();



        }

    }



    public void GetItemInformation(Item item_)
    {

        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (inventory.Container[i].item == item_ && (inventory.Container[i].pNum == PlayerInventoryController.preset_pnum || PlayerInventoryController.preset_pnum ==0))
            {
                ammount = inventory.Container[i].amount;
                Toolhealthing = inventory.Container[i].item_Health;

            }



        }




    }
}
