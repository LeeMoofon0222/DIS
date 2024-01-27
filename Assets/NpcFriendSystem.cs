using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class NpcFriendSystem : MonoBehaviour
{
    public List<Vector2> IDandValue;

    public int love;

    public List<Item> givebacks;

    public void Awake()
    {
        StartCoroutine(spawnitem());
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ItemObject IO))
        {

            //print(IO.item.ID);
            foreach(var list in IDandValue)
            {
                if (list.x == IO.item.ID)
                {
                    love += (int)list.y;
                    Destroy(IO.gameObject);
                }
            }

        }
    }

    IEnumerator spawnitem()
    {
        if(love >= 20)
        {
            Instantiate(givebacks[2].spawntoscene , transform.position + new Vector3(0 , 1f , 0) ,Quaternion.identity );

        }
        else if(love >= 10)
        {

            Instantiate(givebacks[1].spawntoscene, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        }
        else
        {



        }

        yield return new WaitForSeconds(120);
        StartCoroutine(spawnitem());
    }
}
