using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct biomes
{
    [ColorUsage(true)]
    public Color skyColor;
    [ColorUsage(true)]
    public Color EquatorColor;
    [ColorUsage(true)]
    public Color GroundColor;

    public float equator_height;
    public float equator_smoothness;
}

public class SkyChanger : MonoBehaviour
{

    //public biomes defualt;

    public biomes normal;
    public biomes desert;
    public biomes cherry;
    public biomes valcano;
    

    public Material _skybox;

    bool exitArea;

    public int biomechoose;

    int biomeNum;
    float timescale = 3f;

    public PolyverseSkies skyControl;


    // Start is called before the first frame update
    void Start()
    {
        /* _skybox.SetColor("_TintColor", cherry.skyColor);
         _skybox.SetFloat("_CubemapTransition", cherry.blender);
         _skybox.SetFloat("_Exposure",cherry.exposure);
        */
        switch (biomechoose)
        {
            case 0:
                _skybox.SetColor("_SkyColor", normal.skyColor);
                _skybox.SetColor("_EquatorColor", normal.EquatorColor);
                _skybox.SetColor("_GroundColor", normal.GroundColor);

                _skybox.SetFloat("_EquatorHeight", normal.equator_height);
                _skybox.SetFloat("_EquatorSmoothness", normal.equator_smoothness);
                break;
            case 1:

                break;
            case 2:
                _skybox.SetColor("_SkyColor", cherry.skyColor);
                _skybox.SetColor("_EquatorColor", cherry.EquatorColor);
                _skybox.SetColor("_GroundColor", cherry.GroundColor);

                _skybox.SetFloat("_EquatorHeight", cherry.equator_height);
                _skybox.SetFloat("_EquatorSmoothness", cherry.equator_smoothness);
                break;
            case 3:

                break;



        }

        
        



    }

    // Update is called once per frame
    void Update()
    {
        if (exitArea)
        {/*
            _skybox.SetColor("_TintColor", Vector4.Lerp(_skybox.GetColor("_TintColor"), normal.skyColor, Time.deltaTime));
            _skybox.SetFloat("_CubemapTransition", Mathf.Lerp(_skybox.GetFloat("_CubemapTransition"), normal.blender, Time.deltaTime / 3f));
            _skybox.SetFloat("_Exposure", Mathf.Lerp(_skybox.GetFloat("_Exposure"), normal.exposure, Time.deltaTime / 3f));

        */
            _skybox.SetColor("_SkyColor", Vector4.Lerp(_skybox.GetColor("_SkyColor"), normal.skyColor, Time.deltaTime));
            _skybox.SetColor("_EquatorColor", Vector4.Lerp(_skybox.GetColor("_EquatorColor"), normal.EquatorColor, Time.deltaTime));
            _skybox.SetColor("_GroundColor", Vector4.Lerp(_skybox.GetColor("_GroundColor"), normal.GroundColor, Time.deltaTime));

            _skybox.SetFloat("_EquatorHeight", Mathf.Lerp(_skybox.GetFloat("_EquatorHeight"), normal.equator_height, Time.deltaTime /timescale));
            _skybox.SetFloat("_EquatorSmoothness", Mathf.Lerp(_skybox.GetFloat("_EquatorSmoothness"), normal.equator_smoothness, Time.deltaTime / timescale));
        }
        else
        {
            switch (biomeNum)
            {
                case 0:
                    _skybox.SetColor("_SkyColor", Vector4.Lerp(_skybox.GetColor("_SkyColor"), cherry.skyColor, Time.deltaTime));
                    _skybox.SetColor("_EquatorColor", Vector4.Lerp(_skybox.GetColor("_EquatorColor"), cherry.EquatorColor, Time.deltaTime));
                    _skybox.SetColor("_GroundColor", Vector4.Lerp(_skybox.GetColor("_GroundColor"), cherry.GroundColor, Time.deltaTime));

                    _skybox.SetFloat("_EquatorHeight", Mathf.Lerp(_skybox.GetFloat("_EquatorHeight"), cherry.equator_height, Time.deltaTime / timescale));
                    _skybox.SetFloat("_EquatorSmoothness", Mathf.Lerp(_skybox.GetFloat("_EquatorSmoothness"), cherry.equator_smoothness, Time.deltaTime / timescale));
                    break;
                case 1:
                    _skybox.SetColor("_SkyColor", Vector4.Lerp(_skybox.GetColor("_SkyColor"), desert.skyColor, Time.deltaTime));
                    _skybox.SetColor("_EquatorColor", Vector4.Lerp(_skybox.GetColor("_EquatorColor"), desert.EquatorColor, Time.deltaTime));
                    _skybox.SetColor("_GroundColor", Vector4.Lerp(_skybox.GetColor("_GroundColor"), desert.GroundColor, Time.deltaTime));

                    _skybox.SetFloat("_EquatorHeight", Mathf.Lerp(_skybox.GetFloat("_EquatorHeight"), desert.equator_height, Time.deltaTime / timescale));
                    _skybox.SetFloat("_EquatorSmoothness", Mathf.Lerp(_skybox.GetFloat("_EquatorSmoothness"), desert.equator_smoothness, Time.deltaTime / timescale));
                    break;
                case 2:
                    _skybox.SetColor("_SkyColor", Vector4.Lerp(_skybox.GetColor("_SkyColor"), valcano.skyColor, Time.deltaTime));
                    _skybox.SetColor("_EquatorColor", Vector4.Lerp(_skybox.GetColor("_EquatorColor"), valcano.EquatorColor, Time.deltaTime));
                    _skybox.SetColor("_GroundColor", Vector4.Lerp(_skybox.GetColor("_GroundColor"), valcano.GroundColor, Time.deltaTime));

                    _skybox.SetFloat("_EquatorHeight", Mathf.Lerp(_skybox.GetFloat("_EquatorHeight"), valcano.equator_height, Time.deltaTime / timescale));
                    _skybox.SetFloat("_EquatorSmoothness", Mathf.Lerp(_skybox.GetFloat("_EquatorSmoothness"), valcano.equator_smoothness, Time.deltaTime / timescale));

                    break;


            }
        }


        skyControl.timeOfDay = Mathf.PingPong(Time.time / 600, 1);




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




