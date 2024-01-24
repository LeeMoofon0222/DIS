
using UnityEngine;

public class OnHandVFX : MonoBehaviour
{
    public ParticleSystem vfx;


    public void OnCall()
    {
        vfx.Play();


    }
}
