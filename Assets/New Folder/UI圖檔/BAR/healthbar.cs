using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class healthbar : MonoBehaviour
{
    public static healthbar Instance;
    public GameObject[] heart;
    public int currentlife;
    public int maxlife;

    public bool[] boolhealth;
    public GameObject LowhealthPanel;


    // Start is called before the first frame update\
    void Awake()
    {
        Instance = this;
        currentlife = maxlife;
        LowhealthPanel.SetActive(false);
    }
    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            boolhealth[i] = true;
            heart[i].gameObject.SetActive(true);

        }
    }



    // Update is called once per frame
    void Update()
    {




        if (currentlife > maxlife)
        {
            currentlife = maxlife;


        }

        for (int k = 1; k <= 20; k++)
        {
            if (currentlife < k)
            {

                heart[k - 1].gameObject.SetActive(false);
                boolhealth[k - 1] = false;

            }


        }
        for (int n = 0; n < 20; n++)
        {

            if (currentlife > n && currentlife < n + 2 && boolhealth[n] == false)
            {
                for (int a = 0; a < currentlife; a++)
                {
                    heart[a].gameObject.SetActive(true);

                    boolhealth[a] = true;
                }
            }


        }


        if (currentlife <= 6)
        {

            LowhealthPanel.SetActive(true);

        }
        else
        {
            LowhealthPanel.SetActive(false);


        }


    }



    /* if(life < 1)
     {
         Destroy(heart[0].gameObject);


     }
     else if(life<2)
     {
         Destory(heart[1].gameObject);



     }*/
    public void Dead()
    {
        Debug.Log("you dead bc you idiot");

    }







    public void increase_health(int i)
    {
        currentlife += i;

    }
    public void TakeDamage(int d)
    {
        currentlife -= d;

        if (currentlife < 1)
        {
            Dead();
            currentlife = 0;

        }




    }
}
