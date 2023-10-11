using System.Collections;
using System.Collections.Generic;

using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class HopeController : MonoBehaviour
{

    private Vector3 moveRange;
    public float trackingDistance = 5f;
    private GameObject target;
    public float speed;
    private Rigidbody rb;
    
    public Animator anim;
    
    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        float flowspeed = Random.Range(0.7f, 1.3f);
        anim.SetFloat("Speed", flowspeed);
    
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance < trackingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 2 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "pond")
        {






        }
        if (other.gameObject.tag == "Player")
        {
          
            GameObject SystemINformation = GameObject.Find("HopeManger");
            HopeSpawnManger HopeSpawnManger = SystemINformation.GetComponent<HopeSpawnManger>();
            if (HopeSpawnManger != null)
            {
                HopeSpawnManger.HopeValue++;
                Destroy(gameObject);
            }
        }
    }
}
