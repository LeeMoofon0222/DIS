using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public List<float> lastTimer;
    public List<float> Max_buffTime;
    public List<bool> buffTrigger;

    int maxBuff = 5;
    int buffNum;

    PlayerMoveMent PM;
    PlayerControl PC;

    
    // Start is called before the first frame update
    void Start()
    {
        PM = GetComponent<PlayerMoveMent>();
        PC = GetComponent<PlayerControl>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void testBuff(int p)
    {
        if (buffTrigger[p] && lastTimer[p] < Max_buffTime[p])
        {
            //§Þ¯à¼W¯q


        }else
        {
            buffTrigger[p] = false;
            lastTimer[p] = 0;

        }

    }


}
