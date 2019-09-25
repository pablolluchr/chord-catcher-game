using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundController : MonoBehaviour
{
    public float pitch;
    public void Play()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.pitch = pitch;
        source.PlayOneShot(source.clip);
    }
}
