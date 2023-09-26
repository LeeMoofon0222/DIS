using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraLook : MonoBehaviour
{
    public Transform playerBody;

    public float mouseSensitivity;
    
    float mX, mY;

    private float camRotation,playerRotation;
    Transform m_camera;
    public PlayerMoveMent pm;
    public PlayerControl pc;

    public Animator camAnim;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_camera = this.gameObject.transform;
       
    }

    // Update is called once per frame
    void Update()
    {
        

        mX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * -1f;
        

        camRotation += mY;
        playerRotation += mX;
        camRotation = Mathf.Clamp(camRotation, -90f, 90f);

        Quaternion newCamRotation = Quaternion.Euler(camRotation, m_camera.localRotation.y, m_camera.localRotation.z);

        Quaternion newPlayerRotation = Quaternion.Euler(playerBody.localRotation.x, playerRotation, playerBody.localRotation.z);


        m_camera.localRotation = Quaternion.Lerp(m_camera.localRotation,newCamRotation,Time.deltaTime * mouseSensitivity);

        playerBody.localRotation = Quaternion.Lerp(playerBody.localRotation, newPlayerRotation, Time.deltaTime * mouseSensitivity);

        camAnim.SetBool("Moving", (pm.isMoving || pc.attacking));



    }

    
}
