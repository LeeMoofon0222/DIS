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

    public CursorControl cursorControl;
    
    public List<Button> buttons;
    
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
            foreach(var button in buttons)
                button.enabled= true;
            
            //pm.canMove = false; 
            
            cl.enabled = false;
            /*Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;*/
            cursorControl.wc_cursor = true;
            anim.SetBool("OpenWeaponWheel", true);

        }
        else
        {
            foreach (var button in buttons)
                button.enabled = false;


            //pm.canMove = true;
            cl.enabled = true;
            
            anim.SetBool("OpenWeaponWheel", false);
            /*Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;*/
            cursorControl.wc_cursor = false;

        }
    }
}
