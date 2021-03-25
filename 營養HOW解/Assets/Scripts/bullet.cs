using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        rb.velocity=transform.right*speed;
        Destroy(gameObject,4f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="enemy")
        {
            Destroy(gameObject);
        }
    }
}
