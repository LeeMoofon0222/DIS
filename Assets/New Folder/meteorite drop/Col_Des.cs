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
        //var pos = _planeObj.transform.position;
        //Destroy(_planeObj);
        //Instantiate(stone_crack, _planeMovePos[_posIndex].transform.position, Quaternion.identity);
        //_planeObj.GetComponent<Rigidbody>().isKinematic = true;
        mete = true;
        var pos = gameObject.transform.position;
        Destroy(gameObject);
        GameObject obj = Instantiate(stone_crack, pos, Quaternion.identity);
        Destroy(obj,10);
        var ce = GameObject.Find("PlayerCamera").GetComponent<CameraEffects>();
        ce.Shake();
        //ª±®a¦©¦å
    }
}
