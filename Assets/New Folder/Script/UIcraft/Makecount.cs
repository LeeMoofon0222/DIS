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
    // Start is called before the first frame update

    void Start()
    {
        Count = 0;
        Count_text.text = Count.ToString();

    }
    public void Plus()
    {
        Count++;
        Count_text.text = Count.ToString();


    }
    public void Minus()
    {
        Count--;
        Count_text.text = Count.ToString();



    }

    public void MakeCraft()
    {




    }



}