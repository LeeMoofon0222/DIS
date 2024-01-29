using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public Transform target; // 追逐目标
    public float speed = 5.0f; // 移动速度
    public float gravity = 9.8f; // 重力加速度
    public float detectDistance = 15.0f;
    public float stoppingDistance = 3.0f; // 停止距离
    public float stoppingAngle = 20f;
    public float HP = 100.0f;

    public bool isfriendly;

    Rigidbody rb;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    

    public Animator anim;

    float timer;
    
    bool isDead;

    public bool forcedtoDead;
    Quaternion setRotate;

    PlayerHealth playerhealth;

    [Header("Guard")]
    public bool enable_guard;
    public Transform guard;
    [SerializeField]
    private float guardDistence = 30f;

    bool goBack;


    [Header("Attack")]
    public float attackCD;
    public int attackValue;
    GameObject player;
    public LayerMask playermask;
    public GameObject area;
    bool canMove;
    bool is_attacking;
    public GameObject AttackArea;
    public GameObject Attackpoint;

    //[Header("Hurt")]
    private float HurtCD = 0.1f;
    private bool canbeInjured = true;

    public Item loot;
    

    public GameObject hurtparticle;
    public ParticleSystem attackParticle;


    [Header("Ducky")]
    public bool isDucky;
    public Transform movepos;
    Vector3 originPos;
    float backDistance = 30f;
    public LayerMask itemmask;



    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        canMove = true;

        if (!isDucky)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            playerhealth = target.GetComponent<PlayerHealth>();
            player = target.gameObject;
        }
        else
        {
            target = movepos;
            originPos = transform.position;

            StartCoroutine(DuckyMove());
        }
        

    }

    void Update()
    {
        if(forcedtoDead) HP= 0;

        timer += Time.deltaTime;
        if (HP <= 0)
        {
            anim.SetBool("isChaserDetected", false);
            anim.SetBool("isIdle", false);
            anim.SetBool("isAttack", false);
            anim.SetBool("isDead", true);

            Die();
        }
        if (guard == null) guard = this.transform;

        if (target != null && Vector3.Distance(target.position, this.transform.position) <= detectDistance && !isDead &&
            ((!enable_guard || Vector3.Distance(guard.position, this.transform.position) <= 30 && !goBack) ))
        {

            //Debug.Log("gay");
            // 计算NPC角色向目标移动的方向和距离
            Vector3 direction = target.position - transform.position;
            float distance = direction.magnitude;
            direction.y = 0;

            //float angle = Mathf.Abs(transform.rotation.y - target.transform.rotation.y);

            // 如果距离大于停止距离，移动NPC角色
            if (distance > stoppingDistance)
            {
                anim.SetBool("isChaserDetected", true);
                anim.SetBool("isIdle", false);
                anim.SetBool("isAttack", false);
                // 旋转NPC角色，面向目标
                if (!is_attacking) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 15.0f);

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


                    if (!isfriendly)
                    {
                        anim.SetTrigger("atk");

                        StartCoroutine(StoneGolemattack());
                        timer = 0;

                        area.SetActive(true);
                    }
                    

                }

            }
        }
        if (!isDucky)
        {
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
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 15.0f);

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
        }
        else
        {

            /*if (anim.GetBool("isIdle"))
            {
                var _playerpos = GameObject.FindGameObjectWithTag("Player").transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_playerpos), Time.deltaTime * 3.0f);
            }*/

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
            if(loot != null)
            {
                GameObject l = Instantiate(loot.spawntoscene, this.transform.position + new Vector3(0, .5f, 0), Quaternion.identity);
                l.GetComponent<Rigidbody>().isKinematic = true;
                l.layer = 0;
            }
            

            setRotate = transform.rotation;
            controller.enabled = false;
            StartCoroutine(dead());

            Destroy(this.gameObject, 15.0f);
        }
    }
    /*
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
    */
    public void OnHurt(int _damage)
    {
        HP -= _damage;


    }




    IEnumerator dead()
    {
        

        yield return new WaitForSeconds(8f);
        rb.isKinematic = false;

    }

    IEnumerator StoneGolemattack()
    {
        GameObject Attackpartical = Instantiate(AttackArea, Attackpoint.transform.position, transform.rotation);

        canMove = false;
        is_attacking = true;
        yield return new WaitForSeconds(3);
        Destroy(Attackpartical);
        Collider[] collide = Physics.OverlapSphere(area.transform.position, area.GetComponent<SphereCollider>().radius, playermask);
        attackParticle.Play();
        //Debug.Log(collide);
        if (collide.Length > 0) playerhealth.currentlife -= attackValue;

        yield return new WaitForSeconds(attackCD - 3);
        canMove= true;
        is_attacking = false;



    }

    IEnumerator DuckyMove()
    {
        if (isDucky)
        {
            
            int mode = Random.Range(0, 2);

            print(mode);

            Collider[] col = Physics.OverlapSphere(transform.position, 4, itemmask);
            
            if(col.Length != 0)
            {
                target.position = col[0].transform.position;
                movepos = target;
                originPos= target.position;

            }
            else
            {
                if (mode == 1)
                {
                    if (Vector3.Distance(transform.position, originPos) < backDistance)
                    {
                        movepos.transform.localPosition = new Vector3(Random.Range(4f, 8f),
                                                         transform.localPosition.y,
                                                         Random.Range(4f, 8f));
                        target = movepos;

                        print("set");
                    }
                    else
                    {
                        target.position = originPos;
                        print("back");
                    }

                }
                else
                {
                    movepos.transform.localPosition = transform.localPosition;
                    target = movepos;
                    print("idle");

                }
            }



            

            yield return new WaitForSeconds(Random.Range(5, 10));
            StartCoroutine(DuckyMove());
        }
        

    }



}