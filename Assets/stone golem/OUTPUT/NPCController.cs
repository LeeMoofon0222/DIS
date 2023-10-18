using System.Collections;
using UnityEngine;

public class stonegloem : MonoBehaviour
{
    public GameObject chaser;

    public float chaseDistance = 15.0f;
    private bool isIdle = false;
    private bool isChaserDetected = false;
    private bool isDead = false;
    
    public float turnSpeed = 10.0f;

    private Animator animator;

    Rigidbody rb;

    Quaternion setRotate;

    public Transform detector;
    public LayerMask obstacble;

    float _time;
    [Header("CharactorController")]
    public float Speed = 3f;
    public float gravity;
    Vector3 npcVelocity;
    private CharacterController controller;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        isDead = false;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, chaser.transform.position);
        //Debug.Log("Distance : " + distance);

        if (chaser == null)
        {
            Idle();
        }
        else
        {
            if (distance <= chaseDistance)
            {
                isChaserDetected = true;
                isIdle = false;

                Run();
            }
            else if (distance >= 2 * chaseDistance)
            {
                isChaserDetected = false;
                isIdle = true;

                Idle();
            }
            else
            {
                isChaserDetected = false;
                isIdle = true;

                Idle();
            }
        }

        // Update the animator parameters based on the current state
        animator.SetBool("isChaserDetected", isChaserDetected);
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isDead", isDead);

        npcVelocity.y += rb.mass * gravity * Time.deltaTime;
        if (controller.isGrounded)
        {
            npcVelocity.y = -2f;
        }

        controller.Move(npcVelocity);

        if (isDead)
        {
            transform.rotation = setRotate;
        }

    }

    private void Idle()
    {
        if (!isIdle)
        {
            isIdle = true;
        }
    }

    private void Run()
    {
        // Calculate the direction vector from the runner to the chaser
        Vector3 directionToChaser = chaser.transform.position - transform.position;

        // Calculate the angle between the current forward direction and the direction to the chaser
        float angle = Vector3.SignedAngle(transform.forward, directionToChaser, Vector3.up);

        // Gradually turn towards the direction to the chaser
        float turnAmount = Mathf.Sign(angle) * Mathf.Min(Mathf.Abs(angle), turnSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up, turnAmount);

        // Move the runner forward in the new direction using CharacterController
        Vector3 moveDirection = transform.forward * Speed * Time.deltaTime;
        controller.Move(moveDirection);
    
        switch (CheckTurning())
        {
            case 0:
                break;
            case 1:
                transform.Rotate(Vector3.up, 30 * Time.deltaTime * turnSpeed);
                break;
            case 2:
                transform.Rotate(Vector3.up, -30 * Time.deltaTime * turnSpeed);
                break;
            case 3:
                transform.Rotate(Vector3.up, 30 * Time.deltaTime * turnSpeed);
                break;
        }

        // Move the runner forward in the new direction using CharacterController
        moveDirection = transform.forward * Speed * Time.deltaTime;
        controller.Move(moveDirection);
    }

    private void Die()
    {
        if (!isDead)
        {
            isDead = true;
            isChaserDetected = false;
            isIdle = false;
            rb.isKinematic = true;
            rb.freezeRotation = true;

            Debug.LogWarning("dead");

            animator.SetBool("isDead", isDead);
            setRotate = transform.rotation;
            controller.enabled = false;
            StartCoroutine(dead());

            Destroy(this.gameObject, 15.0f);
        }
    }

    int CheckTurning()
    {
        if(Physics.Raycast(detector.position , transform.forward, 3f , obstacble))
        {
            if (Physics.Raycast(detector.position, transform.right, 3f, obstacble))
            {
                return 2;
            }
            else if(Physics.Raycast(detector.position, transform.right * -1, 3f, obstacble))
            {
                return 3;
            }
            else
            {
                return 1;
            }
        }
        else
        {
            return 0;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "chaser" && !isDead)
        {
            Die();
        }
    }

    IEnumerator dead()
    {
        yield return new WaitForSeconds(8f);
        rb.isKinematic = false;


    }
}