using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TRadeManger : MonoBehaviour
{
    
    //public Prompt prompt;
    public GameObject TradeButtonPos;
    public Button[] TradeButton;
    public Button currentButton1;

    public GameObject TradeObject;
    public int CurrentButton = 0;
    public bool onTrigger = false;
    public TradeButtonManger[] tradeButtonMangers;
    public int[] num;
    public GameObject perfab;
    GameObject InstantiateObject;
    //public Sprite BeselectSprite;

    
    public Transform generatepoint;

    // Start is called before the first frame update
    void Awake()
    {
        TradeButton[0].Select();
        TradeObject.SetActive(false);

        InstantiateObject = Instantiate(perfab);
        currentButton1 = Instantiate(TradeButton[CurrentButton], TradeButtonPos.transform).GetComponent<Button>();
      
    }
    void Start()
    {


        
    }

    // Update is called once per frame
    void Update()
    {
        for (int n = 0; n < tradeButtonMangers.Length; n++)
        {
            tradeButtonMangers[n].GetComponent<TradeButtonManger>().enabled = true;

        }
        if (!onTrigger)
            return;

        SetButtonSelect();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (CurrentButton == 0)
                return;

            Destroy(currentButton1.gameObject);
            currentButton1 = Instantiate(TradeButton[--CurrentButton], TradeButtonPos.transform).GetComponent<Button>();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (CurrentButton == TradeButton.Length - 1)
                return;

            Destroy(currentButton1.gameObject);
            currentButton1 = Instantiate(TradeButton[++CurrentButton], TradeButtonPos.transform).GetComponent<Button>();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            CLickTrade();
        }

    }
    public void SetButtonSelect()
    {
        currentButton1.Select();

    }
    public void CLickTrade()
    {
        TradeButton[CurrentButton].onClick.Invoke();
    }
    public void RandomButton()
    {
        /*num = new int[7];
        prompt.cant = false;
        Array.Clear(num, 0, num.Length);
        /*for (int i = 0; i < 7; i++)
        {

            int RandomRange = UnityEngine.Random.Range(0, tradeButtonMangers.Length);
            num[i] = RandomRange;
            Debug.Log(num[i]);
            for (int n=1; n < num.Length; n++)
            {   if (num.Length == 1)
                    break;
                if (num[i] == num[n-1] )
                {
                    i--;
                    //n = num.Length;
                }
            }

        }
        for (int i = 0; i < 7; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, tradeButtonMangers.Length);

            // 檢查是否已經存在於數組中
            bool isDuplicate = false;
            for (int n = 0; n < i; n++)
            {
                if (num[n] == randomIndex)
                {
                    isDuplicate = true;
                    break;
                }
            }

            // 如果是重複的索引，則重新選擇一個隨機索引
            if (isDuplicate)
            {
                i--;
                continue;
            }

            num[i] = randomIndex;
            Debug.Log(num[i]);
        }
        for (int n = 0; n < TradeButton.Length; n++)
        {
            //TradeButton[n].transform.gameObject.SetActive(false);
            Destroy(TradeButton[n]);
        }
        for (int i = 0; i < num.Length; i++)
        {
            //TradeButton[num[i]].transform.gameObject.SetActive(true);
            Instantiate(TradeButton[num[i]], TradeButtonPos.transform);
        }*/

    }
}
