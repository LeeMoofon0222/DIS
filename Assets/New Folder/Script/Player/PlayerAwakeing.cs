using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;


public class PlayerAwakeing : MonoBehaviour
{
    public Image img;

    public Volume volume;
    Vignette vignette;
    ColorAdjustments exposure;
    float _exposure;

    public bool respawnable = false;

    float u;
    [HideInInspector]public float timer;

    PlayerMoveMent pm;
    PlayerHealth playerhealth;

    public void Awake()
    {
        img.color = new Vector4(0, 0, 0, 255);
        pm= GetComponent<PlayerMoveMent>();
        volume.profile.TryGet<Vignette>(out vignette);
        vignette.intensity.value = 1;

        volume.profile.TryGet<ColorAdjustments>(out exposure);
        _exposure = exposure.postExposure.value;
        exposure.postExposure.value = 0f;

        playerhealth = GetComponent<PlayerHealth>();

        respawnable = true;
    }

    public void Update()
    {
        if (respawnable)
        {
            if (timer < 10f) timer += Time.deltaTime;
            img.color = new Vector4(0, 0, 0, Mathf.Lerp(1, 0, timer * 2));

            volume.profile.TryGet<Vignette>(out vignette);
            u = Mathf.Lerp(1, 0.15f, timer / Mathf.Lerp(1f, 3, timer *1200));
            vignette.intensity.value = u;

            volume.profile.TryGet<ColorAdjustments>(out exposure);
            exposure.postExposure.value = Mathf.Lerp(0, _exposure, timer);

        }
        else
        {   
            if(timer < 10f ) timer += Time.deltaTime;
            img.color = new Vector4(0, 0, 0, Mathf.Lerp(0, 1, timer * 3));
            
        }

    }
    public void wakeUp()
    {
        timer = 0f;
        respawnable = false;

        StartCoroutine(deadcountDown());
    }

    IEnumerator deadcountDown()
    {
        //pm.canMove = false;
        yield return new WaitForSeconds(2f);
        timer = 0f;

        img.color = new Vector4(0, 0, 0, 255);

        volume.profile.TryGet<Vignette>(out vignette);
        vignette.intensity.value = 1;


        volume.profile.TryGet<ColorAdjustments>(out exposure);
        exposure.postExposure.value = 0f;

        pm.cC.enabled = false;
        pm.player.transform.position = pm.respawnPoint;
        pm.cC.enabled = true;

        respawnable = true;
        pm.isDead = false;
        playerhealth.currentlife = playerhealth.maxlife;
        //pm.canMove = true;
    }
}
