using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControl : MonoBehaviour
{
    public bool pc_cursor;
    public bool wc_cursor;
    public bool cam_cursor;
    public bool pic_cursor;
    public bool setting_cursor;


    private void FixedUpdate()
    {
        if(pc_cursor || wc_cursor || cam_cursor || pic_cursor || setting_cursor)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;


        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
