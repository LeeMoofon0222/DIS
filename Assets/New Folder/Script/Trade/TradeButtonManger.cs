using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TradeButtonManger : MonoBehaviour
{
    public Button TradeButton;
    public Item TradeItem;
    public Item GetTRadeItem;
    public HopeSpawnManger HopeSpawnManger;
    public int HopeCost;
   
    public int TradeAmount;
    public int GetTradeAmount;

    public InventoryRecord inventoryRecord;
    public TRadeManger tradeManger;
    
    // Start is called before the first frame update
    void Awake()
    {

       /* tradeManger = GameObject.FindGameObjectWithTag("Hook").GetComponent<TRadeManger>(); 
        fishing = tradeManger.GetComponentInChildren<Fishing>();*/
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void GetTrade()
    {
        GameObject SystemINformation = GameObject.Find("HopeManger");
        HopeSpawnManger = SystemINformation.GetComponent<HopeSpawnManger>();


        for(int i = 0;i<inventoryRecord.Container.Count;i++)
        {
            if (inventoryRecord.Container[i].item == TradeItem && inventoryRecord.Container[i].amount >=TradeAmount)
            {
                inventoryRecord.DecreesItem(TradeItem, TradeAmount, 0);
                inventoryRecord.AddItem(GetTRadeItem, GetTradeAmount, GetTRadeItem.itemHealth, 0);
            }



        }
      
        if(HopeSpawnManger.HopeValue >= HopeCost)
        {
            HopeSpawnManger.HopeValue -= HopeCost;
            inventoryRecord.AddItem(GetTRadeItem, GetTradeAmount, GetTRadeItem.itemHealth, 0);


        }


    }
   
    
}
