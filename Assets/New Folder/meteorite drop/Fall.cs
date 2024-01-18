using DitzeGames.Effects;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fall : MonoBehaviour
{
    public GameObject Meteorite;
    public Transform SpawnMeteoritePosition;
    GameObject _planeObj;
    public GameObject[] _planeMovePos;
    //public Camera _camera;
    private int _posIndex;
    private bool Mov;
    public bool vol=false;
    GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }


    public void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= 50 && vol)
        {
            StartCoroutine(SpawnMeteorite());
        }
        else if(Vector3.Distance(player.transform.position, transform.position) > 50)
        {
            vol = true;
        }
        if (Mov)
        {
            MovePos(_posIndex);
        }
    }
    void MovePos(int _posIndex)
    {
        //print(_posIndex);
        _planeObj.transform.position = Vector3.MoveTowards(_planeObj.transform.position, _planeMovePos[_posIndex].transform.position, 20 * Time.deltaTime);
         if (Vector3.Distance(_planeMovePos[_posIndex].transform.position, _planeObj.transform.position) < 0.0000f)
         {
             Mov = false;
         }
    }

    IEnumerator SpawnMeteorite()
    {
        Mov = false;
        vol = false;
        _planeObj = Instantiate(Meteorite, SpawnMeteoritePosition.position, Quaternion.identity);
        _posIndex = Random.Range(0, _planeMovePos.Length);
        Mov = true;
        float SpawnTime = Random.Range(6, 15);
        yield return new WaitForSeconds(SpawnTime);
        if(Vector3.Distance(player.transform.position, transform.position) <= 50)
        {
            yield return SpawnMeteorite();
        }
    }
}
