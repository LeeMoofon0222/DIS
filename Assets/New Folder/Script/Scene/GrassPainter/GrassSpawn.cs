using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawn : MonoBehaviour
{
    public GameObject grass;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject Grass = Instantiate(grass, this.transform.position, this.transform.rotation);
    }
}
