using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject Player;

    private void Awake()
    {
        Instantiate(Player, this.transform);
    }
}
