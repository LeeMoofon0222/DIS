using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct biomes
{
    [ColorUsage(true)]
    public Color skyColor;

    public float exposure;
    public float blender;
}

public class SkyChanger : MonoBehaviour
{
    public biomes normal;
    public biomes desert;
    public biomes cherry;
    public biomes valcano;

    public Material _skybox;

    bool exitArea;

    int biomeNum;


    // Start is called before the first frame update
    void Start()
    {
        _skybox.SetColor("_TintColor", cherry.skyColor);
        _skybox.SetFloat("_CubemapTransition", cherry.blender);
        _skybox.SetFloat("_Exposure",cherry.exposure);
    }

    // Update is called once per frame
    void Update()
    {
        if (exitArea)
        {
            _skybox.SetColor("_TintColor", Vector4.Lerp(_skybox.GetColor("_TintColor"), normal.skyColor, Time.deltaTime));
            _skybox.SetFloat("_CubemapTransition", Mathf.Lerp(_skybox.GetFloat("_CubemapTransition"), normal.blender, Time.deltaTime / 3f));
            _skybox.SetFloat("_Exposure", Mathf.Lerp(_skybox.GetFloat("_Exposure"), normal.exposure, Time.deltaTime / 3f));
        }
        else
        {
            switch (biomeNum)
            {
                case 0:
                    _skybox.SetColor("_TintColor", Vector4.Lerp(_skybox.GetColor("_TintColor"), cherry.skyColor, Time.deltaTime / 3f));
                    _skybox.SetFloat("_CubemapTransition", Mathf.Lerp(_skybox.GetFloat("_CubemapTransition"), cherry.blender, Time.deltaTime / 3f));
                    _skybox.SetFloat("_Exposure", Mathf.Lerp(_skybox.GetFloat("_Exposure"), cherry.exposure ,Time.deltaTime / 3f));
                    break;
                case 1:
                    _skybox.SetColor("_TintColor", Vector4.Lerp(_skybox.GetColor("_TintColor"), desert.skyColor, Time.deltaTime / 3f));
                    _skybox.SetFloat("_CubemapTransition", Mathf.Lerp(_skybox.GetFloat("_CubemapTransition"), desert.blender, Time.deltaTime / 3f));
                    _skybox.SetFloat("_Exposure", Mathf.Lerp(_skybox.GetFloat("_Exposure"), desert.exposure, Time.deltaTime / 3f));
                    break;
                case 2:
                    _skybox.SetColor("_TintColor", Vector4.Lerp(_skybox.GetColor("_TintColor"), valcano.skyColor, Time.deltaTime / 3f));
                    _skybox.SetFloat("_CubemapTransition", Mathf.Lerp(_skybox.GetFloat("_CubemapTransition"), valcano.blender, Time.deltaTime / 3f));
                    _skybox.SetFloat("_Exposure", Mathf.Lerp(_skybox.GetFloat("_Exposure"), valcano.exposure, Time.deltaTime / 3f));

                    break;


            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("cherryIsland"))
        {
            biomeNum = 0;
            exitArea = false;
            //_skybox.SetColor("_TintColor", Vector4.Lerp(_skybox.GetColor("_TintColor"), cherry.skyColor, Time.deltaTime));
        }

        if (other.CompareTag("desertIsland"))
        {
            biomeNum = 1;
            exitArea = false;
            //_skybox.SetColor("_TintColor", Vector4.Lerp(_skybox.GetColor("_TintColor"), cherry.skyColor, Time.deltaTime));
        }

        if (other.CompareTag("lavaIsland"))
        {
            biomeNum = 2;
            exitArea = false;
            //_skybox.SetColor("_TintColor", Vector4.Lerp(_skybox.GetColor("_TintColor"), cherry.skyColor, Time.deltaTime));
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (   other.CompareTag("cherryIsland")
            || other.CompareTag("desertIsland")
            || other.CompareTag("lavaIsland"))
        {
            exitArea = true;
        }
        


    }

}




