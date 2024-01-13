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

    public GameObject Player;
    public Camera FovCamera;
    private PlayerControl PlayerScript;
    private PlayerCameraLook PlayerCameraScript;

    public Volume volume;
    MotionBlur blur;

    public TMP_Text MotionBlur_ButtonTag;
    public GameObject MotionBlur_Obj;

    public Slider Sensitivity;
    public Slider FOV;

    // Start is called before the first frame update
    void Start()
    {
        SetGround.SetActive(false);

        PlayerScript = Player.GetComponent<PlayerControl>();
 
        Transform cameraHolderTransform = Player.transform.Find("CameraHolder");
        PlayerCameraScript = cameraHolderTransform.GetComponent<PlayerCameraLook>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))//KeyCode.Escape
        {
            Player.GetComponent<PlayerControl>().SettingmodeForRotate(true);

            SetGround.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        PlayerCameraScript.SettingSensitivity(Sensitivity.value);
        FovCamera.fieldOfView = FOV.value * 60 + 30;
    }



    public void CloseSetting()
    {
        Player.GetComponent<PlayerControl>().SettingmodeForRotate(false);

        SetGround.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

    public void SettingClose()
    {
        PlayerScript.AbsoluteControlRotateButton(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        SetGround.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
