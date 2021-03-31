using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundmanager : MonoBehaviour
{
    public static soundmanager instance;
    public AudioSource audioSource;
    public AudioClip jumpAudio,hurtAudio,poopAudio;
    private void Awake()
    {
        instance=this;
    }
    public void Jumpaudio()
    {
        audioSource.clip=jumpAudio;
        audioSource.Play();
    }
    public void Hurtaudio()
    {
        audioSource.clip=hurtAudio;
        audioSource.Play();
    }
    public void Poopaudio()
    {
        audioSource.clip=poopAudio;
        audioSource.Play();
    }
}
