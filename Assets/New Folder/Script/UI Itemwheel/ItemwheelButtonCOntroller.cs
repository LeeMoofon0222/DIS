//using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using UnityEngine;
using UnityEngine.UI;

public class ItemwheelButtonCOntroller : MonoBehaviour
{
    private Animator anim;

    public  GameObject selectedItem;
    private bool selected = false;
    public GameObject ButtonImage;
    public GameObject ItemFieldicon;
    //public int pcint;
    public PlayerControl playerCOntrol;
    public PlayerInventoryController pc;
    //public ItemwheelButtonCOntroller Instance;


    // Start is
    // called before the first frame update
    private void Awake()
    {
        //Instance = this; 
    }
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            selectedItem.GetComponent<Image>().sprite = ButtonImage.GetComponent<Image>().sprite;

        }
    }

    public void Selected()
    {
        selected = true;

    }

    public void DEselected()
    {
        selected = false;

    }

    public void HoverEnter()
    {

        anim.SetBool("Hover", true);

    }


    public void HoverExit()
    {

        anim.SetBool("Hover", false);

    }

    public void Settingicon()
    {
        ButtonImage.GetComponent<Image>().sprite = ItemFieldicon.GetComponent<Image>().sprite;
        Debug.Log("test1");

    }
    public void SetOnHand()
    {
        StartCoroutine(changehand());

    }

    IEnumerator changehand()
    {
        playerCOntrol.handHolderController.SetTrigger("change");
        yield return new WaitForSeconds(0.6f);
        playerCOntrol.RepairItemOnHand();
    }

}
