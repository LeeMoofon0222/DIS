using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct islands
{
    public GameObject checkingBox;
    public GameObject blockIsland;

    public CreateRecipe recipe;
}


public class BlockCrossing : MonoBehaviour
{
    public GameObject checkingBox;
    [SerializeField] List<islands> Island;

    public GameObject blockIsland;
    Vector3 recordLast;

    public Transform Player;

    public Transform linestart;

    public Transform bridgeHolder;

    float nowmw;
    float _nowmw;
    public int setnum;
    public int _setnum;

    public List<Transform> prebridge;

    Vector3 firstBridgePos;
    Vector3 EndBridgePos;

    Vector3[] points = {new Vector3(0,0,0), new Vector3(0, 0, 0 )};

    public GameObject line;
    public GameObject cam;

    public LineRenderer baseLine;

    public LayerMask cantBridge;

    public int state;


    bool isspawned;
    GameObject _checker;

    public InventoryRecord m_inventory;

    private void Awake()
    {
        state= 0;
        isspawned= false;
    }

    // Update is called once per frame
    void Update()
    {
        float mw = Input.GetAxis("Mouse ScrollWheel");
        nowmw += mw * 10f;
        _nowmw = Mathf.Repeat(nowmw, Island.Count - 0.5f);
        nowmw = Mathf.Repeat(nowmw, 9.5f);

        setnum = Mathf.RoundToInt(nowmw);
        _setnum = Mathf.RoundToInt(_nowmw);

        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            state = state == 1 ? 0 : 1;
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            /*state++;
            if (state > 1) state = 0;*/
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            /*state--;
            if (state < 0) state = 1;*/
        }


        

        if(state == 1)
        {
            #region multibridging
            isspawned = false;
            Destroy(_checker);

            line.SetActive(true);

            firstBridgePos = new Vector3(linestart.position.x, Player.position.y - 1.5f, linestart.position.z);
            //firstBridgePos = new Vector3(Player.transform.position.x, Player.position.y - 4f, firstPrebridge.transform.localPosition.z);
            EndBridgePos = new Vector3(prebridge[setnum].position.x, Player.position.y - 1.5f, prebridge[setnum].position.z)
                            + new Vector3(Mathf.Sin(Player.eulerAngles.y * Mathf.Deg2Rad) * 2.5f, 0f, Mathf.Cos(Player.eulerAngles.y * Mathf.Deg2Rad) * 2.5f);

            //Debug.Log(Mathf.Sin(Player.eulerAngles.y * Mathf.Deg2Rad)); ;

            

            points[0] = firstBridgePos;
            points[1] = EndBridgePos;

            if(setnum <= 0)
            {
                baseLine.enabled= false;
            }
            else
            {
                baseLine.enabled = true;
            }
            baseLine.SetPositions(points);


            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(spawingTick());
            }
            #endregion
        }
        else
        {
            line.SetActive(false);

            if (!isspawned)
            {
                _checker = Instantiate(Island[_setnum].checkingBox, bridgeHolder.position, Player.rotation) as GameObject;
                isspawned= true;
            }
            _checker.transform.position = new Vector3(bridgeHolder.position.x , bridgeHolder.position.y - 5.12f, bridgeHolder.position.z);
            _checker.transform.rotation = Player.rotation;
            Collider[] checkcollide = Physics.OverlapBox(_checker.transform.position, _checker.GetComponent<BoxCollider>().size / 2, Player.rotation, cantBridge);

            if (checkcollide.Length == 0)
            {
                _checker.transform.GetChild(0).GetComponent<Renderer>().material = _checker.transform.GetChild(0).GetComponent<preMaterial>().Mat[1];
                
                if (Input.GetMouseButton(0))
                {
                    if (checkResource())
                    {
                        Instantiate(Island[_setnum].blockIsland, new Vector3(bridgeHolder.position.x, bridgeHolder.position.y - 5.12f, bridgeHolder.position.z), Player.rotation);
                        Destroy(_checker);
                        isspawned = false;
                        for(int i =0; i < Island[setnum].recipe.materials.Count; i++)
                        {
                            m_inventory.DecreesItem(Island[setnum].recipe.materials[i].ID , Island[setnum].recipe.materials[i].amount);
                        }
                        
                    }

                    //else ¤£°÷ªº¿é¥X
                }
            }
            else
            {
                _checker.transform.GetChild(0).GetComponent<Renderer>().material = _checker.transform.GetChild(0).GetComponent<preMaterial>().Mat[0];
            }

        }
    }




    IEnumerator spawingTick()
    {
        //GameObject firstpre = Instantiate(blockIsland, firstBridgePos, Player.transform.rotation);
        //prebridgeNum.Add(firstpre.transform);

        Quaternion _angle = Player.transform.rotation;
        List<Vector3> setpos = new List<Vector3>();

        foreach (var pos in prebridge)
        {
            setpos.Add(new Vector3(pos.transform.position.x, Player.position.y - 5.12f, pos.transform.position.z));
        }

        for (int i = 0; i < setnum; i++)
        {
            GameObject checker = Instantiate(checkingBox, setpos[i], _angle);

            Collider[] checkcollide = Physics.OverlapBox(checker.transform.position, checker.GetComponent<BoxCollider>().size / 2,_angle, cantBridge);


            if (checkcollide.Length == 0)
            {
                if (checkResource())
                {
                    Instantiate(blockIsland, setpos[i], _angle);
                    yield return new WaitForSeconds(0.5f);
                    Destroy(checker);
                    for (int j = 0; j < Island[0].recipe.materials.Count; j++)
                    {
                        m_inventory.DecreesItem(Island[0].recipe.materials[j].ID, Island[0].recipe.materials[j].amount);
                    }
                }

            }
            else
            {
                /*foreach (var t in checkcollide)
                {
                    Debug.Log(t);
                }*/


                Destroy(checker);
            }
            //prebridgeNum.Add(pre.transform);
        }
    }
    public bool checkResource()
    {
        for (int i = 0; i < Island[0].recipe.materials.Count; i++)
        {
            if (!m_inventory.FindItem(Island[0].recipe.materials[i].ID, Island[0].recipe.materials[i].amount))
            {
                return false;
            }
        }
        return true;
    }

    void OnDisable()
    {
        /*GameObject[] preisland = GameObject.FindGameObjectsWithTag("preisland");
       
        foreach(var obj in preisland)
        {
            Destroy(obj);
        }*/
        isspawned = false;
        state = 0;
        if (_checker != null) Destroy(_checker);
    }
}
