using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnvilUIpanel : MonoBehaviour
{
    public GameObject CraftButtonPos;
    public Button[] CraftButton;
    public Button currentButton1;
    public Prompt prompt;
    public GameObject CraftObject;
    public int CurrentButton = 0;
    public bool onTrigger;
    public GameObject ScrollView;
    int n = 0;
    public int[] num;
   

    // Start is called before the first frame update
    void Awake()
    {
        CraftButton[0].Select();
        //ScrollView.transform.position = new Vector2(0, ScrollView.transform.position.y);
        CraftObject.SetActive(false);

        currentButton1 = Instantiate(CraftButton[CurrentButton], CraftButtonPos.transform).GetComponent<Button>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!onTrigger)
            return;
        SetButtonSelect();

        /*if (Input.GetKeyDown(KeyCode.LeftArrow) && n<0)
        {
            if (CurrentButton == 0)
                return;
            n += 1;
            ScrollView.transform.position = new Vector2(ScrollView.transform.position.x + 400, ScrollView.transform.position.y);
            Debug.Log(ScrollView.transform.position);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && n > -400*(CraftButton.Length - 1))
        {
            if (CurrentButton == CraftButton.Length - 1)
                return;
            n -= 1;
            ScrollView.transform.position = new Vector2(ScrollView.transform.position.x - 400, ScrollView.transform.position.y);
            Debug.Log(ScrollView.transform.position);
        }*/


        if (Input.GetKeyDown(KeyCode.RightArrow) && n > -400 * (CraftButton.Length - 1))
        {
            // Move the scroll view to the right
            n -= 400;
            ScrollView.transform.localPosition = new Vector2(ScrollView.transform.localPosition.x - 400, ScrollView.transform.localPosition.y);
            Debug.Log(ScrollView.transform.position);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && n < 0)
        {
            n += 400;
            ScrollView.transform.localPosition = new Vector2(ScrollView.transform.localPosition.x + 400, ScrollView.transform.localPosition.y);
            Debug.Log(ScrollView.transform.localPosition);
        }


        if (Input.GetKeyDown(KeyCode.Return))
        {
            ClickCraft();
        }


    }



    public void SetButtonSelect()
    {
        currentButton1.Select();
    }
    public void ClickCraft()
    {
        CraftButton[CurrentButton].onClick.Invoke();

    }
}
