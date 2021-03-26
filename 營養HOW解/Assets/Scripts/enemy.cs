using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    protected Animator anim;
    protected AudioSource deathAudio;
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        deathAudio=GetComponent<AudioSource>();
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    public void JumpOn()
    {
        anim.SetTrigger("death");
        deathAudio.Play();
    }
}
