using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class barController : MonoBehaviour
{

    /*public Slider hungerbar;
    public Slider energybar;*/
    public Image hungerbar;
    public Image energybar;

    public PlayerMoveMent pm;

    public static barController Instance;

    bool reCall , _reCall;

    public float regen = 0;

    void Awake()
    {
        Instance = this;
        reCall = true;
        _reCall = true;

        hungerbar = GameObject.Find("飽食度fill").GetComponent<Image>();
        energybar = GameObject.Find("精力條fill").GetComponent<Image>();

    }
    public void SetMaxhunger(float hunger)
    {
        hungerbar.fillAmount = hunger;

    }
    public void Sethunger(float hunger)
    {

        //hungerbar.fillAmount = hunger;

    }
    public void EatFood(float value)
    {

        hungerbar.fillAmount += value;

    }
    public void SetMaxenergy(float energy)
    {
        energybar.fillAmount = energy;

    }
    public void Setenergy(float energy)
    {
        energybar.fillAmount = energy;
    }

    void Update()
    {
        if (pm.isSprinting() && pm.isMoving && _reCall)
        {
            StartCoroutine(decreesEnergy());
            regen = 0;
        }
        else if (reCall && _reCall)
        {
            StartCoroutine(Reenergy());
        }
    }


    IEnumerator Reenergy()
    {
        reCall = false;
        yield return new WaitForSeconds(0.75f);

        regen++;
        energybar.fillAmount = Mathf.Lerp(0, 1, (float)(energybar.fillAmount + regen * 0.03));

        if (energybar.fillAmount > 1)
        {
            energybar.fillAmount = 1;
        }

        reCall = true;


    }

    IEnumerator decreesEnergy()
    {
        _reCall = false;
        reCall = false;
        yield return new WaitForSeconds(0.5f);
        energybar.fillAmount = Mathf.Lerp(0, 1, (float)(energybar.fillAmount - 0.02));

        if (energybar.fillAmount < 0)
        {
            energybar.fillAmount = 0;
        }
        reCall = true;
        _reCall = true;


    }
}