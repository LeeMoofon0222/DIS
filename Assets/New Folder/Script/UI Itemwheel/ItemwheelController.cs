using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemwheelController : MonoBehaviour
{

    public PlayerCameraLook cl;
    public Animator anim;
    public  bool weaponWheelSelected = false;
    public Image selectedItem;
    public Sprite noImage;
    public static int weaponId;
    public List<ItemwheelButtonCOntroller> itembutton;
    public PlayerMoveMent pm;
    public PlayerInventoryController playerInventoryController;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && playerInventoryController.inventoryOpen == false)  
        {

            weaponWheelSelected = !weaponWheelSelected;
            for(int i =0; i<5; i++) 
            {
                itembutton[i].Settingicon();
                //Debug.Log("test");
            }

           

        }


        if (weaponWheelSelected )
        {
            //pm.canMove = false; 
            Cursor.visible = true;
            cl.enabled = false;
            Cursor.lockState = CursorLockMode.Confined;
            anim.SetBool("OpenWeaponWheel", true);
           

        }
        else
        {
            //pm.canMove = true;
            cl.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            anim.SetBool("OpenWeaponWheel", false);
           

        }

    }


}
