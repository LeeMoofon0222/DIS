using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;


public class Prompt : MonoBehaviour
{
    public bool cant = true;
    Ray raycast;
    public Animator anim;
    public Animator panel;
    bool Getkey;
    [SerializeField] private Transform rayOrigin;
    public Camera Camera;
    bool canpickup;
    public GameObject prompt;
    public Item item;
    GameObject player;
    //public GameObject pos;
    public ItemObject ItemObject;
    public float ItemObjectHealth;
    public Text INformationText;
    public Text Name;
<<<<<<< Updated upstream
    public Text TypeText;
=======
    public Text Type;
>>>>>>> Stashed changes

    [Header("NPC")]
    public Text NPChealth;
    public Text NPCattack;
    public Text NPCtypes;
    public GameObject NPC;
    private NPCObject npcObject;

    [Header("Loot Item")]
    public Text ItemWeight;
    public Text Toolhealth;
    public Text Foodhealth;
    public Text Itemattack;
    public Text ItemDEF;
    public Text eating;
    public Text healing;
    public GameObject Food;
    public GameObject Tool;
    public GameObject ItemMain;
    public GameObject Item;
    private ToolObject toolObject;
    private FoodObject foodObject;
    private LootObject lootObject;
    public Text period;
    public GameObject Plant;
    private PlantObject plantObject;
    int Toolhealthing;

    [Header("Dig Object")]
    public Text Objectheal;
    public Text ObjectLoot;
    /*bool isDig;
    public Image isDigImage;
    public Sprite CanDig;
    public Sprite CantDig;*/
    TRadeManger trade;
    public GameObject Dig;
    bool once = true;
    public Image ItemIcon;

    AnvilUIpanel craft;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        CloseAllObject();
    }
    void CloseAllObject()
    {
        Dig.SetActive(false);
        Item.SetActive(false);
        NPC.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        Vector3 pos = player.transform.position;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        RaycastHit hit;
        //Ray ray = Camera.ViewportPointToRay(pos);
        //Ray ray = new Ray(Camera.transform.position, forward);
        //Ray ray = new Ray(pos, player.transform.forward);
        //bool isHit = Physics.Raycast(ray, out hit, 5f);



        raycast.origin = rayOrigin.position;
        raycast.direction = rayOrigin.forward;
        bool ishit = Physics.Raycast(raycast, out hit, 5f);
        Debug.DrawRay(rayOrigin.position, rayOrigin.forward, Color.red);




        if (ishit)
        {
            if (hit.collider.gameObject.TryGetComponent(out AnvilUIpanel craft_))
            {
                if(craft_.CraftObject != null)
                    craft_.CraftObject.SetActive(true);

                craft_.onTrigger = true;

                craft = craft_;
            }
            else
            {
                if(craft_  != null)
                {
                    craft_.onTrigger = false;

                    if (craft_.CraftObject != null)
                        craft_.CraftObject.SetActive(false);


                }


            }
            if (hit.collider.gameObject.TryGetComponent(out TRadeManger Trade))
            {
                Trade.TradeObject.SetActive(true);
                Trade.onTrigger = true;
                trade = Trade;
                if (cant == true)
                {


                    Trade.RandomButton();


                }



            }
            else
            {
                if (trade != null)
                {
                    cant = true;
                    trade.onTrigger = false;
                    trade.TradeObject.SetActive(false);
                }
            }


            if (hit.collider.gameObject.TryGetComponent(out ItemObject itemObject))     //0503
            {
                if (once == true)
                {

                    StartCoroutine(ChangeAlph());
                    once = false;

                }


                //prompt.SetActive(true);
                //Debug.Log(itemObject.objectHealth);
                
                SetItemType(itemObject);
                canpickup = itemObject.canPick;
                ItemObject = itemObject;
                ItemObjectHealth = itemObject.objectHealth;

                if (Input.GetKeyDown(KeyCode.Mouse2))
                {
                    anim.SetBool("openPrompt", !Getkey);
                    Getkey = !Getkey;

                }
            }
            else
            {
                once = true;

            }





        }
        else
        {
            if (craft != null)
            {
                craft.onTrigger = false;
                craft.CraftObject.SetActive(false);


            }

            anim.SetBool("openPrompt", false);

        }



    }
    IEnumerator ChangeAlph()
    {
        panel.SetBool("Change", true);
        yield return new WaitForSeconds(1f);
        panel.SetBool("Change", false);



    }
    void SetItemType(ItemObject itemObject)
    {

        var _item = itemObject.item;    //0503
        item = _item;
        if (itemObject.canPick)
        {
            ItemIcon.sprite = _item.itemIcon;
<<<<<<< Updated upstream
            TypeText.text = "���ʼҦ�:�i�B��";
=======
            Type.text = "���ʼҦ�:�i�B��";
>>>>>>> Stashed changes
        }
        else
        {
            ItemIcon.sprite = itemObject.m_sprite;
<<<<<<< Updated upstream
            TypeText.text = "���ʼҦ�:�i�}�a";
=======
            Type.text = "���ʼҦ�:�i�}�a";
>>>>>>> Stashed changes
        }
        
        if (_item.type == ItemType.Tool)
        {
            ItemMainSeting();
            
            Tool.SetActive(true);
           

            toolObject = (ToolObject)_item;
            Itemattack.text = "�ˮ`:" + toolObject.ToolATK.ToString();
            ItemDEF.text = "���m:" + toolObject.ToolDEF.ToString();
            Name.text = itemObject.m_name == null ? _item.itemName : itemObject.m_name;
            INformationText.text = _item.desciption;
            Toolhealth.text = "�@�[:" + item.itemHealth.ToString();

        }
        else if (_item.type == ItemType.Food)
        {
            ItemMainSeting();

            Food.SetActive(true);
            foodObject = (FoodObject)_item;
            eating.text = "������:" + foodObject.satiety.ToString();
            healing.text = "�^��q:" + foodObject.healing.ToString();
            Name.text = itemObject.m_name == null ? _item.itemName : itemObject.m_name;
            INformationText.text = _item.desciption;
            Foodhealth.text = "�@�[:" + item.itemHealth.ToString();

            

        }
        else if (_item.type == ItemType.Loot)
        {
            if (canpickup)
            {

                ItemMainSeting();
                Name.text = itemObject.m_name == null ? _item.itemName : itemObject.m_name;
                INformationText.text = _item.desciption;
            }
            else
            {
                CloseAllObject();
                Dig.SetActive(true);
                lootObject = (LootObject)_item;
                Objectheal.text = "��q:" + ItemObjectHealth.ToString();
                ObjectLoot.text = "������:" + lootObject.Drop;
                Name.text = itemObject.m_name == null ? _item.itemName :itemObject.m_name;
                INformationText.text = _item.desciption;

            }


        }
        else if (_item.type == ItemType.NPC)
        {
            CloseAllObject();
            NPC.SetActive(true);
            npcObject = (NPCObject)_item;
            NPChealth.text = "��q:" + npcObject.Health.ToString();
            NPCattack.text = "�ˮ`:" + npcObject.attack.ToString();
            NPCtypes.text = "�欰�Ҧ�:" + npcObject.NPCType;
            Name.text = itemObject.m_name == null ? _item.itemName : itemObject.m_name;
            INformationText.text = _item.desciption;

        }
        else if (_item.type == ItemType.Plant)
        {
            ItemMainSeting();
            Plant.SetActive(true);
            plantObject = (PlantObject)_item;
            period.text = "������:" + plantObject.period.ToString();
            Name.text = itemObject.m_name == null ? _item.itemName : itemObject.m_name;
        }




    }
    void ItemMainSeting()
    {

        CloseAllObject();
        Item.SetActive(true);
        ItemMain.SetActive(true);
        ItemWeight.text = "���q:" + item.itemWeights.ToString();
        CloseALLItem();


    }
    void CloseALLItem()
    {
        Plant.SetActive(false);
        Food.SetActive(false);
        Tool.SetActive(false);
       


    }
}
