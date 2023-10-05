using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBarController : MonoBehaviour
{

    public float Maxhunger;
    public float Maxenergy;
    public float currenthunger;
    public float currentenergy;
    public barController bar;

    // Start is called before the first frame update
    void Start()
    {
        currentenergy =Maxenergy;
        currenthunger =Maxhunger;
        bar.SetMaxenergy(Maxenergy);
        bar.SetMaxhunger(Maxhunger);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Takehunger(int hunger)
    {
        currenthunger -= hunger; 
        bar.Sethunger(currenthunger);


    }
    void Takeenergy(int energy)
    { 
        currentenergy -= energy;
        bar.Setenergy(currentenergy);


    }
}
