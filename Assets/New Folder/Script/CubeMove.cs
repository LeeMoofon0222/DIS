using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeMove : MonoBehaviour
{
    public List<Transform> leftpoint;
    public List<Transform> rightpoint;
    public List<Transform> uppoint;
    public List<Transform> downpoint;

    public bool canmove;
    public bool canThrogh;
    int i;

    // Update is called once per frame
    void Update()
    {
        if (canmove)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                foreach (Transform t in uppoint)
                {
                    if (Physics.Raycast(t.transform.position, t.transform.forward,out RaycastHit hit ,  .7f))
                    {
                        
                        if(hit.transform.CompareTag("w") && canThrogh)
                        {
                            print("a");
                        }
                        else 
                        {
                            return;
                        }
                        
                    }
                }
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 1f);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                foreach (Transform t in uppoint)
                {
                    if (Physics.Raycast(t.transform.position, t.transform.right * -1, .7f))
                    {
                        print("a");
                        return;
                    }
                }
                transform.localPosition = new Vector3(transform.localPosition.x - 1f, transform.localPosition.y, transform.localPosition.z);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                foreach (Transform t in downpoint)
                {
                    if (Physics.Raycast(t.transform.position, t.transform.forward * -1, out RaycastHit hit, .7f))
                    {
                        if (hit.transform.CompareTag("w") && canThrogh)
                        {
                            print("a");
                            i++;
                        }
                        else
                        {
                            i = 0;
                            return;
                        }
                    }
                }
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 1f);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                foreach (Transform t in uppoint)
                {
                    if (Physics.Raycast(t.transform.position, t.transform.right, .7f))
                    {
                        print("a");
                        return;
                    }
                }
                transform.localPosition = new Vector3(transform.localPosition.x + 1, transform.localPosition.y, transform.localPosition.z);
            }

            if( i >= 2)
            {
                print("reward");
            }
        }
    }



    public  void trigger()
    {
        GameObject[] c = GameObject.FindGameObjectsWithTag("c");
        foreach(var itm in c)
        {
            itm.GetComponent<CubeMove>().canmove = false;
        }
        canmove = true;
        
    }
}
