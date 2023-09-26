using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    

    //public GameObject floor;
    

    Vector3 point;
    Vector3 lastpoint;

    public List<Vector3> spawned_pos;
    public GameObject[] island;

    public GameObject player;
    
    public int IslandSpawningMagnifaction;
    //int maxIslands;


    int posX, posZ;

   void Start()
    {
        player = GameObject.FindWithTag("Player");

        point = player.transform.position;

        spawned_pos[0] = new Vector3(0,0,0);

        posX = 0;
        posZ = 0;

        Instantiate(island[0], new Vector3(posX, 0f, posZ), transform.rotation);





    }

    void Update()
    {
        player = GameObject.FindWithTag("Player");

        lastpoint = player.transform.position;

        

        if(lastpoint.x - point.x > 50 && posX - point.x <= IslandSpawningMagnifaction)
        {
            posX += IslandSpawningMagnifaction;
        }
        if (lastpoint.x - point.x < -50 && posZ - point.z <= IslandSpawningMagnifaction)
        {
            posX -= IslandSpawningMagnifaction;
        }
        if (lastpoint.z - point.z > 50)
        {
            posZ += IslandSpawningMagnifaction;
        }
        if (lastpoint.z - point.z < -50)
        {
            posZ -= IslandSpawningMagnifaction;
        }


        point = new Vector3(posX, 0f, posZ);

        generator();



    }

    public bool checkSpawned(Vector3 point)
    {
        int cnt = 0;

        for (int i= 0; i < spawned_pos.Count; i++)
        {
            if(point.x == spawned_pos[i].x && point.z == spawned_pos[i].z)
            {
                cnt++;
            }
        }

        if (cnt > 0) 
        {

            return true;
        }
        else
        {
            return false;
        }
        

        
    }

    void generator()
    {
        if (!checkSpawned(point))
        {
            

            Instantiate(island[Random.Range(1, island.Length)], point, transform.rotation);

            spawned_pos.Add(point);
        }
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {   
                Vector3 generation_point = new Vector3(point.x + i * IslandSpawningMagnifaction, Random.Range(-100f,100f), point.z + j * IslandSpawningMagnifaction);

                if (!checkSpawned(generation_point))
                {
                    Instantiate(island[Random.Range(1, island.Length)], generation_point, Quaternion.Euler(0f,Random.Range(0,359),0f)) ;

                    spawned_pos.Add(generation_point);
                }
            }
        }



    }





}
