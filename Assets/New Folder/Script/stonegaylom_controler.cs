using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stonegaylom_controler : MonoBehaviour
{
    public Transform target; // 追逐目标
    public float speed = 5.0f; // 移动速度
    public float gravity = 9.8f; // 重力加速度
    public float detectDistance = 15.0f;
    public float stoppingDistance = 3.0f; // 停止距离
    public float HP = 100.0f;
    Rigidbody rb;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    

    public Animator anim;

    float timer;
    
    bool isDead;
    Quaternion setRotate;

    public PlayerHealth playerhealth;

    [Header("Guard")]
    public bool enable_guard;
    public Transform guard;
    [SerializeField]
    private float guardDistence = 30f;

    bool goBack;


    [Header("Attack")]
    public float attackCD;
    public int attackValue;
    public GameObject player;
    public LayerMask playermask;
    public GameObject area;
    bool canMove;
    bool is_attacking;
    public GameObject AttackArea;
    public GameObject Attackpoint;

    //[Header("Hurt")]
    private float HurtCD = 0.1f;
    private bool canbeInjured = true;





    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        canMove = true;

        target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void Update()
    {
        timer += Time.deltaTime;
        if (HP <= 0)
        {
            anim.SetBool("isChaserDetected", false);
            anim.SetBool("isIdle", false);
            anim.SetBool("isAttack", false);
            anim.SetBool("isDead", true);

            Die();
        }

        if (target != null && Vector3.Distance(target.position, this.transform.position) <= detectDistance && !isDead &&
            ((Vector3.Distance(guard.position, this.transform.position) <= 30 && !goBack) || !enable_guard))
        {

            Debug.Log("gay");
            // 计算NPC角色向目标移动的方向和距离
            Vector3 direction = target.position - transform.position;
            float distance = direction.magnitude;
            direction.y = 0;

            // 如果距离大于停止距离，移动NPC角色
            if (distance > stoppingDistance)
            {
                anim.SetBool("isChaserDetected", true);
                anim.SetBool("isIdle", false);
                anim.SetBool("isAttack", false);
                // 旋转NPC角色，面向目标
                if (!is_attacking) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5.0f);

                // 移动NPC角色
                if (canMove) moveDirection = direction.normalized * speed;


            }
            else if (!isDead)
            {
                // 距离小于停止距离时，停止移动
                moveDirection = Vector3.zero;
                //ATTACK

                if (timer >= attackCD && !is_attacking)
                {
                    anim.SetBool("isChaserDetected", false);
                    anim.SetBool("isIdle", true);
                    anim.SetTrigger("atk");

                    StartCoroutine(attack());
                    timer = 0;

                    area.SetActive(true);

                }

            }
        }

        if (!isDead && Vector3.Distance(target.position, this.transform.position) >= detectDistance)
        {
            anim.SetBool("isAttack", false);
            anim.SetBool("isChaserDetected", false);
            anim.SetBool("isIdle", true);
        }

        if (!isDead && enable_guard && Vector3.Distance(guard.position, this.transform.position) >= guardDistence)
        {
            goBack = true;
        }

        if (goBack && !is_attacking)
        {
            anim.SetBool("isChaserDetected", true);
            anim.SetBool("isIdle", false);

            Vector3 direction = guard.position - transform.position;
            float distance = direction.magnitude;
            direction.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5.0f);

            // 移动NPC角色
            moveDirection = direction.normalized * speed;
        }
        if (Vector3.Distance(guard.position, this.transform.position) <= 10)
        {
            goBack = false;
        }

        if (is_attacking)
        {
            anim.SetBool("isChaserDetected", false);
        }

        // 计算重力
        moveDirection.y -= gravity;

        // 移动NPC角色
        controller.Move(moveDirection * Time.deltaTime);


        if (timer >= HurtCD)
        {
            canbeInjured = true;
        }
    }

    private void Die()
    {
        if (!isDead)
        {
            isDead = true;
            rb.isKinematic = true;
            rb.freezeRotation = true;

            //Debug.LogWarning("dead");

            
            setRotate = transform.rotation;
            controller.enabled = false;
            StartCoroutine(dead());

            Destroy(this.gameObject, 15.0f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ItemObject io) && canbeInjured)
        {
            if(other.gameObject.layer == 6 )
            {
                canbeInjured = false;
                
                if(io.item.type == ItemType.Tool)
                {
                    ToolObject weapon = (ToolObject)io.item;
                    HP -= weapon.ToolATK;
                }
                else
                {
                    HP -= 1;
                }
            }
        }
    }





    IEnumerator dead()
    {
        yield return new WaitForSeconds(8f);
        rb.isKinematic = false;

    }

    IEnumerator attack()
    {
        GameObject Attackpartical = Instantiate(AttackArea, Attackpoint.transform.position, transform.rotation);

        canMove = false;
        is_attacking = true;
        yield return new WaitForSeconds(3);
        Destroy(Attackpartical);
        Collider[] collide = Physics.OverlapSphere(area.transform.position, area.GetComponent<SphereCollider>().radius, playermask);
        Debug.Log(collide);
        if (collide.Length > 0) playerhealth.currentlife -= attackValue;

        yield return new WaitForSeconds(attackCD - 3);
        canMove= true;
        is_attacking = false;



    }

}