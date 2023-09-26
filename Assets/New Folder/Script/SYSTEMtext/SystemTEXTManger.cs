using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SystemTEXTManger : MonoBehaviour
{
    public GameObject[] GetItemText;
    public GameObject GetItemTextPErfab;
    int n = 0;
    public int GetItemTextCount = 0;

    public GameObject Content;
    public float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        GetItemText = new GameObject[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (GetItemTextCount == GetItemText.Length)
        {
            currentTime += Time.deltaTime;
        }
        if (GetItemTextCount != GetItemText.Length)
        {

            GetItemTextCount = GetItemText.Length;

        }
        StartCoroutine(TimeOut());

    }

    public void Getin(Item _item, int _amount)
    {
        currentTime = 0f;
        if (n >= GetItemText.Length)
        {
            // 如果 GetItemText 陣列已經被填滿，擴展陣列大小
            Array.Resize(ref GetItemText, GetItemText.Length + 1); // 例如，這裡擴展了 1 個遊戲物件的空間
        }

        GetItemText[n] = Instantiate(GetItemTextPErfab, Content.transform);
        var GetItem = GetItemText[n].transform.Find("GetItem");
        var _itemText = GetItem.transform.Find("ItemText").GetComponent<Text>();
        var _itemIcon = GetItem.transform.Find("ItemIcon").GetComponent<Image>();
        _itemText.text = _item.name + "×" + _amount.ToString();
        _itemIcon.sprite = _item.itemIcon;

        n++;
    }

    IEnumerator TimeOut()
    {
        if (GetItemText.Length > 10)
        {
            Destroy(GetItemText[0]);
            for (int i = 0; i < (GetItemText.Length - 1); i++)
                GetItemText[i] = GetItemText[i + 1];
            Array.Resize(ref GetItemText, GetItemText.Length - 1);
            n = 10;


        }


        
        if (currentTime > 5f)
        {
            for (int i = 0; i < GetItemText.Length; i++)
            {
                /* if (GetItemText[i] == null)
                     break;*/
                if (GetItemText[i] != null)
                {
                    var SystemImageManger = GetItemText[i].GetComponent<SystemImageManger>();
                    SystemImageManger.anim.Play("GetOut");

                }



            }
            if (GetItemText[GetItemText.Length - 1] != null)
            {
                var LastSystemImageManger = GetItemText[GetItemText.Length - 1].GetComponent<SystemImageManger>();
                yield return new WaitForSeconds(LastSystemImageManger.anim.GetCurrentAnimatorStateInfo(0).length);
            }
            
            
            for (int m = 0; m < GetItemText.Length; m++)
            {
                if (GetItemText[m] != null)
                {
                    Destroy(GetItemText[m]);
                }
            }
            GetItemText = new GameObject[1];
            Debug.Log("Clear the array");
            currentTime = 0f;

            n = 0;

        }


    }
}
