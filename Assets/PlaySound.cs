using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioClip clip;
    public bool loop;
    
    public float time;
    public float volume;

    private void Awake()
    {
        StartCoroutine(playsound());
    }
    IEnumerator playsound()
    {
        AudioSource.PlayClipAtPoint(clip, transform.position,volume);
        yield return new WaitForSeconds(time);
        StartCoroutine(playsound());
    }

}
