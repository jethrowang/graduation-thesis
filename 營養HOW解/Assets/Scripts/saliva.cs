using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saliva : MonoBehaviour
{
    public GameObject saliva_particles;
    void Start()
    {
        Destroy(gameObject, 7f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player") || collision.gameObject.CompareTag("platform")||collision.gameObject.CompareTag("enemy"))
        {
            Destroy(this.gameObject);
            Instantiate(saliva_particles,transform.position,Quaternion.identity);
        }
    }
}
