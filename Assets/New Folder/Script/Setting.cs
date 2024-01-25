using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class Setting : MonoBehaviour
{
    public GameObject SetGround;
    public GameObject cursor;
    public GameObject Player;
    public Camera FovCamera;

    private PlayerControl PlayerScript;
    private PlayerCameraLook PlayerCameraScript;
    public CursorControl cursorControl;

    public bool opening = false;
    public Volume volume;
    MotionBlur blur;

    public TMP_Text MotionBlur_ButtonTag;
    public GameObject MotionBlur_Obj;

    public Slider Sensitivity;
    public Slider FOV;
    public ItemwheelController ItemwheelController;
    public PlayerInventoryController PIC;
    public ItemwheelController wc;
    public PlayerControl PC;
    // Start is called before the first frame update
    void Start()
    {
        SetGround.SetActive(false);

        PlayerScript = Player.GetComponent<PlayerControl>();
 
        Transform cameraHolderTransform = Player.transform.Find("CameraHolder");
        PlayerCameraScript = cameraHolderTransform.GetComponent<PlayerCameraLook>();

        /*Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
        cursorControl.setting_cursor = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !PIC.inventoryOpen && !wc.weaponWheelSelected && !PC.gayL)
            opening = !opening;

        
            

        if (opening)//KeyCode.Escape
        { 
            Player.GetComponent<PlayerControl>().SettingmodeForRotate(true);
            cursor.SetActive(false);
            SetGround.SetActive(true);
            //Cursor.lockState = CursorLockMode.Confined;
            //Cursor.visible = true;
            cursorControl.setting_cursor = true;
        }
        else if(!opening && !ItemwheelController.weaponWheelSelected)
        {
            Closing();
        }



        PlayerCameraScript.SettingSensitivity(Sensitivity.value); 
        FovCamera.fieldOfView = FOV.value * 60 + 30;
    }



   

    public void SetMotionBlur()
    {
        volume.profile.TryGet<MotionBlur>(out blur);

        if (MotionBlur_ButtonTag.text == "ON")
        {
            blur.active = false;
            MotionBlur_ButtonTag.text = "OFF";
        }
        else
        {
            MotionBlur_ButtonTag.text = "ON";
            blur.active = true;
        }
    }
    public void Closing()
    {
        opening = false;

        Player.GetComponent<PlayerControl>().SettingmodeForRotate(false);
        cursor.SetActive(true);
        SetGround.SetActive(false);
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        cursorControl.setting_cursor = false;

    }
    /*public void SettingClose()
    {
        PlayerScript.AbsoluteControlRotateButton(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        SetGround.SetActive(false);
    }
    public void CloseSetting()
    {
        Player.GetComponent<PlayerControl>().SettingmodeForRotate(false);

        SetGround.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    */
    public void ExitGame()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}
