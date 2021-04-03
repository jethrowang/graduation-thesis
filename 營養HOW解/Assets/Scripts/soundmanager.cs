using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundmanager : MonoBehaviour
{
    public static soundmanager instance;
    public AudioSource audioSource;
    public AudioClip jumpAudio,hurtAudio,poopAudio,deathAudio,throwAudio;
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
    public void Deathaudio()
    {
        audioSource.clip=deathAudio;
        audioSource.Play();
    }
    public void Throwaudio()
    {
        audioSource.clip=throwAudio;
        audioSource.Play();
    }
}
