using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Makecount : MonoBehaviour
{

    public Text Count_text;
    public Button plus;
    public Button minus;
    public Button craft;
    public int Count;

    public bool limited;
    // Start is called before the first frame update

    void Start()
    {
        Count = 1;
        Count_text.text = Count.ToString();

    }
    public void Plus()
    {

        if (limited)
        {
            Count = 1;
        }
        else
        {
            Count++;
        }
        
        Count_text.text = Count.ToString();


    }

    public void SetMax( bool tf)
    {
        limited= tf;
    }


    public void Minus()
    {
        if(Count == 0)
        {
            return;
        }

        Count--;
        Count_text.text = Count.ToString();



    }

    public void MakeCraft()
    {




    }



}