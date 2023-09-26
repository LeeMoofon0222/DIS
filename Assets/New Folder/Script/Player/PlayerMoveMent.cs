using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveMent : MonoBehaviour
{
    public GameObject player;
    public CharacterController cC;
    public Rigidbody rb;

    float gravity = -9.81f ;
    Vector3 velocity;

    public float speed, originalSpeed;

    float v_input, h_input;

    public float jumpHeight;

    public bool isMoving;

    public bool canMove;


    [Header("Respawn")]
    public float respawnHight;
    public Vector3 respawnPoint;
    PlayerAwakeing PA;


    [Header("UI")]
    public barController barcontroller;


    [Header("Sprinting")]
    [SerializeField] private float sprintingHeadbob;
    [SerializeField] float sprintSpeed_Rate;




    public bool isSprinting() 
    {
        if (Input.GetAxis("Sprint") != 0)
            return true;
        
        return false;
    }

    [Header("HeadBobing")]
    float headbob_x , headbob_y;
    public GameObject cam;
    Vector3 camResetPos;
    
    

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        camResetPos = cam.transform.localPosition;
        canMove = true;
        PA = GetComponent<PlayerAwakeing>();

    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.y  <= respawnHight && PA.deadOrNot)
        {
            PA.wakeUp();
        }

        v_input = Input.GetAxisRaw("Vertical") * speed;
        h_input = Input.GetAxisRaw("Horizontal") * speed;
        if(v_input != 0 || h_input != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }



        if (isSprinting() && barcontroller.energybar.fillAmount > 0)
        {
            speed = originalSpeed * sprintSpeed_Rate;
            sprintingHeadbob = 1.4f;
        }
        else
        {
            speed = originalSpeed;
            sprintingHeadbob = 1;
        }
            
        



        Vector3 move = transform.right * h_input  + transform.forward * v_input;
        if ((Input.GetButtonDown("Jump")) && cC.isGrounded && canMove && barcontroller.energybar.fillAmount >= 0.1)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.81f);

            barcontroller.energybar.fillAmount = Mathf.Lerp(0, 1, (float)(barcontroller.energybar.fillAmount - 0.1));

            barcontroller.regen = 0;
        }

        
        velocity.y += gravity * Time.deltaTime * 1.5f;

        
        if (cC.isGrounded && velocity.y < 0)
        {
            velocity.y = -3f;
        }

        if (canMove)
        {
            cC.Move(move * Time.deltaTime * speed);
        }

        cC.Move(velocity * Time.deltaTime);



        if (isMoving && cC.isGrounded && canMove )
        {
            headbob_x = Mathf.Sin(Time.time * 6 * sprintingHeadbob) * 0.04f;
            headbob_y = (Mathf.Cos(Time.time * 12 * sprintingHeadbob) * 0.08f * -1 ) ;

            if(cam.transform.localPosition == camResetPos)
            {
                cam.transform.localPosition = Vector3.Lerp(camResetPos, new Vector3(headbob_x, headbob_y, 0f), Time.deltaTime );
            }
            else
            {
                cam.transform.localPosition = new Vector3(headbob_x, headbob_y, 0f);
            }
            

        }
        else
        {
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camResetPos,  Time.deltaTime);
        }




    }


    
}
