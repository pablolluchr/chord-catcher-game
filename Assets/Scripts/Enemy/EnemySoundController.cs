using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundController : MonoBehaviour
{
    public int semitones;
    private readonly float step = Mathf.Pow(2, (1f / 12));
    public void Play(int mode)
    {

        //mode = 0: player attacks enemy
        //mode = 1: enemy attacks player 

        AudioSource[] sources = GetComponents<AudioSource>();
        if (mode == 0)
        {
            AudioSource source = sources[0];
            source.pitch = Mathf.Pow(step, semitones);
            source.PlayOneShot(source.clip);
        }
        else
        {
            AudioSource source = sources[1];
            source.pitch = Mathf.Pow(step, semitones);
            source.pitch = source.pitch / Mathf.Pow(step, 12);
            source.PlayOneShot(source.clip);

        }


    }
}
