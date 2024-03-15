using DitzeGames.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Col_Des : MonoBehaviour
{
    public bool mete;
    public GameObject stone_crack;
    public Camera _camera;
    void OnCollisionEnter(Collision collision)
    {
        mete = true;
        var pos = gameObject.transform.position;
        Destroy(gameObject);
        GameObject obj = Instantiate(stone_crack, pos, Quaternion.identity);
        Destroy(obj,10);
        var ce = GameObject.Find("PlayerCamera").GetComponent<CameraEffects>();
        ce.Shake(1,1); //相機震動(秒數,劇烈程度)

        if (collision.transform.CompareTag("Player"))
        {
            var player = GameObject.FindWithTag("Player");
            if (player.TryGetComponent(out PlayerHealth PH)) PH.TakeDamage(10);
            //玩家扣血
        }

    }
}
