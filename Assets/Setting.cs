using System.Collections;
using System.Collections.Generic;
<<<<<<< Updated upstream
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
//using System.Diagnostics.Eventing.Reader;
//using UnityEngine.Rendering.HighDefinition;
=======
using System.Diagnostics;
using UnityEngine;
>>>>>>> Stashed changes

public class Setting : MonoBehaviour
{
    public GameObject SetGround;
<<<<<<< Updated upstream
    public TMP_Text MotionBlur_ButtonTag;
    public GameObject pc;
    public GameObject MotionBlur_Obj;

    public void CloseSetting()
    {
        pc.GetComponent<PlayerControl>().SettingmodeForRotate(false);

        SetGround.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetMotionBlur()
    {
        if (MotionBlur_ButtonTag.text == "ON")
        {
            volume.profile.TryGet<MotionBlur>(out exposure);
            exposure.active = true;
            MotionBlur_ButtonTag.text = "OFF";
        }
        else
        {
            MotionBlur_ButtonTag.text = "ON";
        }

        /*if (profile.TryGet<MotionBlur>(out var motionBlur))
        {
        
        }*/
        //Bool MotionBlur = MotionBlur_Obj.GetComponent<MotionBlur>().enabled;
        //MotionBlur_ButtonTag.text = MotionBlur_Obj.GetComponent<Volume>().MotionBlur.enabled;
    }
=======
    public GameObject Player;
    private PlayerControl PlayerScript;
>>>>>>> Stashed changes

    // Start is called before the first frame update
    void Start()
    {
        SetGround.SetActive(false);
<<<<<<< Updated upstream
=======
        PlayerScript = Player.GetComponent<PlayerControl>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        if (Input.GetKeyDown(KeyCode.Z)){
            pc.GetComponent<PlayerControl>().SettingmodeForRotate(true);

            SetGround.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
=======
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerScript.AbsoluteControlRotateButton(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SetGround.SetActive(true);
        }
    }

    public void SettingClose()
    {
        PlayerScript.AbsoluteControlRotateButton(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        SetGround.SetActive(false);
    }
>>>>>>> Stashed changes
}
