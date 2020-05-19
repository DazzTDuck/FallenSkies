using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GolemFootsteps : MonoBehaviour
{
    public AudioClip[] clips;

    public AudioSource source;

    public void RandomizeSound()
    {
        int index = Random.Range(0, clips.Length);
        source.clip = clips[index];
    }

    public void CallStepSound()
    {
        RandomizeSound();
        source.Play();
    }
}
