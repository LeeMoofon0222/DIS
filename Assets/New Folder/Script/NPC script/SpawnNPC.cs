using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNPC : MonoBehaviour
{

    public Transform[] spawnPoints;
    public GameObject[] NPC;
    public GameObject spawnGameObject;
    int NPcCOunt = 0;
    public int NPcCOUntlimit;
    public float SpawnNPcCD;
    // Start is called before the first frame update
    void Start()
    {
        NPC = new GameObject[1];
  
    }

    // Update is called once per frame
    void Update()
    {
         for(int i=0; i<NPC.Length; i++)
        {
            if(NPC[i] == null)
            {
                var temp = new List<GameObject>(NPC);
                temp.RemoveAt(i);
                NPC = temp.ToArray();
            }
        }
    }
    IEnumerator SPawnNpc()
    {
        while(NPcCOunt <= NPcCOUntlimit)
        {
            yield return new WaitForSeconds(SpawnNPcCD);
            int spawnpoint = UnityEngine.Random.Range(0, spawnPoints.Length);
            Array.Resize(ref NPC, NPC.Length + 1);
            NPC[NPcCOunt] = Instantiate(spawnGameObject, spawnPoints[spawnpoint].position, transform.rotation);
            NPcCOunt++;
        }
        


    }
}
