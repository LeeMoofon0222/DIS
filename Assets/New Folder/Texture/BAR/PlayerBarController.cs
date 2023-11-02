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

    [SerializeField] int hungerTime;
    public float hungrySpeed;

    PlayerMoveMent pm;
    public bool isHunger;

    // Start is called before the first frame update
    void Start()
    {
        currentenergy =Maxenergy;
        currenthunger =Maxhunger;
        bar.SetMaxenergy(Maxenergy);
        bar.SetMaxhunger(Maxhunger);

        StartCoroutine(hunger());

        //pm = GetComponent<PlayerMoveMent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currenthunger <= 0)
        {
            isHunger = true;
        }
    }

    void Takehunger(int hunger)
    {
        currenthunger -= hunger; 
        bar.Sethunger(currenthunger / 100);


    }
    void Takeenergy(int energy)
    { 
        currentenergy -= energy;
        bar.Setenergy(currentenergy);


    }

    IEnumerator hunger()
    {
        yield return new WaitForSeconds(hungerTime);
        Takehunger((int)(5 * hungrySpeed));
        StartCoroutine(hunger());
    }
}
