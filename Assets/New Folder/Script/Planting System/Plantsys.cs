using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Plantsys : MonoBehaviour
{
    public int p; // ��e�Ӫ��ͪ����q����
    private int period; // �Ӫ��ͪ��һݮɶ�
    private int id; // �Ӫ�ID
    private int step; // �Ӫ��ͪ����q�ƶq
    public GameObject plant; // ��e�Ӫ�����
    private float planthealth; // �Ӫ��ͩR��
    public List<GameObject> plantlist; // �Ӫ����P�ͪ����q�w�s��C��
    private GameObject plantpoint; // �شӦ�m
    private GameObject player; // ���a����
    public int watering; // ���a������A
    public bool hasplant = true; // �O�_���Ӫ�
    public bool doplant = true; // �O�_���b�ش�
    public Slider slider; // �ͪ��i�ױ�
    private bool test1 = true; // �Ω󱱨����p�ɾ�

    public void Start()
    {
        slider.gameObject.SetActive(true); // �ҥΥͪ��i�ױ�
    }

    public void Update()
    {
        player = GameObject.FindWithTag("Player"); // ������a����
        watering = player.GetComponent<PlayerControl>().watering; // ������a������A

        // �p�G���a���b����Btest1��true�B���b�ش�
        if (player.GetComponent<PlayerControl>().watering == 1 && test1 && doplant)
        {
            Invoke("ReduceBar", 1); // 1���ե�ReduceBar��k
            test1 = false; // �Ntest1�]��false
        }

        // �p�G���b�شӥB���a���b���
        if (doplant && player.GetComponent<PlayerControl>().watering == 1)
        {
            Destroy(plant, period); // �bperiod�ɶ���P����e�Ӫ�
            Invoke("PlantPlant", period); // �bperiod�ɶ���ե�PlantPlant��k
            doplant = false; // �Ndoplant�]��false
        }

        // �p�G���Ӫ�,�Nhasplant�]��true,�_�h�]��false
        if (plant != null)
        {
            hasplant = true;
        }
        else
        {
            hasplant = false;
            p = 0; // ���m�ͪ����q����
        }

        // �p�G�ͪ��i�ױ���0,���m��period
        if (slider.value == 0)
        {
            slider.value = period;
        }

        // �p�G�ͪ����q���޵���̫�@�Ӷ��q,���åͪ��i�ױ�
        if (p != 0 && p == plantlist.Count)
        {
            slider.gameObject.SetActive(false);
        }

        // �p�G���b�شӪ��A,�]�m���a������A��0
        if (!doplant)
        {
            player.GetComponent<PlayerControl>().watering = 0;
        }

        // �p�G�S���Ӫ�,���åͪ��i�ױ��ó]�mdoplant��false
        if (!hasplant)
        {
            slider.gameObject.SetActive(false);
            doplant = false;
        }
    }

    // �}�l�ش�
    public void DoPlanting(PlantObject thisseed)
    {
        doplant = false; // �]�m���b�شӪ��A
        p = 0; // ���m�ͪ����q����
        period = thisseed.period; // ����Ӫ��ͪ��һݮɶ�
        plantlist = thisseed.plantstep; // ����Ӫ��ͪ����q�w�s��C��
        slider.maxValue = period; // �]�m�ͪ��i�ױ��̤j��
        step = plantlist.Count; // ����ͪ����q�ƶq
        id = thisseed.ID; // ����Ӫ�ID

        // �ھ�ID����شӦ�m
        if (id == 40001)
        {
            plantpoint = gameObject.transform.GetChild(0).gameObject;
        }
        if (id == 40002)
        {
            plantpoint = gameObject.transform.GetChild(1).gameObject;
        }
        if (id == 40003)
        {
            plantpoint = gameObject.transform.GetChild(1).gameObject;
        }
        if (id == 40004)
        {
            plantpoint = gameObject.transform.GetChild(1).gameObject;
        }
        if (id == 40005)
        {
            plantpoint = gameObject.transform.GetChild(0).gameObject;
        }

        // ��ҤƲĤ@�ӥͪ����q
        plant = Instantiate(plantlist[p], plantpoint.transform.position, plantpoint.transform.rotation);
        slider.gameObject.SetActive(true); // ��ܥͪ��i�ױ�
        slider.maxValue = period; // �]�m�ͪ��i�ױ��̤j��
        slider.value = period; // �]�m�ͪ��i�ױ��Ȭ��̤j��
        plant.layer = 0; // �]�m�Ӫ��h��
        gameObject.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>().Play(); // ����شӲɤl�S��
        planthealth = plant.AddComponent<ItemObject>().objectHealth; // ����Ӫ��ͩR��
        plant.GetComponent<Rigidbody>().isKinematic = true; // �]�m�Ӫ����B�ʾǭ���
        plant.GetComponent<ItemObject>().canPick = false; // �]�m�Ӫ����i�Q�߰_
        plant.GetComponent<ItemObject>().PickObj = false; // �]�m�Ӫ����i�Q�߰_
        p += 1; // �W�[�ͪ����q����
        doplant = true; // �]�m���b�شӪ��A
    }

    // �ͦ��U�@�ӥͪ����q
    public void PlantPlant()
    {
        plant = Instantiate(plantlist[p], plantpoint.transform.position, plantlist[p].transform.rotation); // ��ҤƤU�@�ӥͪ����q
        player.GetComponent<PlayerControl>().watering = 0; // �]�m���a������A��0
        slider.gameObject.SetActive(true); // ��ܥͪ��i�ױ�
        slider.maxValue = period; // �]�m�ͪ��i�ױ��̤j��
        slider.value = period; // �]�m�ͪ��i�ױ��Ȭ��̤j��
        test1 = true; // �]�mtest1��true
        gameObject.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>().Play(); // ����شӲɤl�S��

        // �p�G���O�̫�@�ӥͪ����q,�]�m���b�شӪ��A
        if (p < step - 1)
        {
            doplant = true;
        }
        p += 1; // �W�[�ͪ����q����
    }

    // ��֥ͪ��i�ױ���
    public void ReduceBar()
    {
        slider.value -= 1; // ��֥ͪ��i�ױ���
        player.GetComponent<PlayerControl>().watering = 0; // �]�m���a������A��0

        // �p�G�ͪ��i�ױ��Ȥ���0,�b1���A���ե�ReduceBar
        if (slider.value != 0)
        {
            Invoke("ReduceBar", 1);
        }
    }
}