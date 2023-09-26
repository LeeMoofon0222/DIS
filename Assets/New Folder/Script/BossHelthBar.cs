using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class BossHelthBar : MonoBehaviour
{
    GameObject NearBoss;
    GameObject Player;
    public Image BossHealth;
    public GameObject BossBar;
    public NPCController NPCController;
    public LayerMask NPCMask;
    public Collider[] test;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
         
    



    }

    // Update is called once per frame
    void Update()
    {
        Collider[] playerColliders = Physics.OverlapSphere(Player.transform.position, 20f, NPCMask);
        test = new Collider[playerColliders.Length];
        for (int i = 0;i < playerColliders.Length; i++)
        {
            test[i] = playerColliders[i];
        }
         
        if (playerColliders.Length > 0)
        {
            Debug.Log("have npc in range");
            NearBoss = playerColliders[0].gameObject;
            NPCController = NearBoss.GetComponent<NPCController>();
            if (Vector3.Distance(Player.transform.position, NearBoss.transform.position) <= 20)
            {

                BossBar.SetActive(true);
                BossHealth.fillAmount = Mathf.Lerp(0, 1, (float)(NPCController.HP*0.01));

            }
            else
            {
                BossBar.SetActive(false);

            }




        }
        else
        {
            BossBar.SetActive(false);
            NPCController = null;
        }



    }
}
