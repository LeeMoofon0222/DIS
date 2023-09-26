using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemsManager : MonoBehaviour
{
    public List<GameObject> systems;

    public GameObject optionPanel;

    public void setSystemOff()
    {   
        foreach(var system in systems)
        {
            system.SetActive(false);
        }
        //systems.SetActive(true);
    }
    public void callSystem(int systenNum)
    {
        setSystemOff();
        systems[systenNum].SetActive(true);
        optionPanel.SetActive(false);


    }
}
