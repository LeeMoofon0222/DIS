using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
//using System.Diagnostics.Eventing.Reader;
//using UnityEngine.Rendering.HighDefinition;

public class Setting : MonoBehaviour
{
    public GameObject SetGround;
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

    // Start is called before the first frame update
    void Start()
    {
        SetGround.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)){
            pc.GetComponent<PlayerControl>().SettingmodeForRotate(true);

            SetGround.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
