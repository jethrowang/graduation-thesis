using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bacterial : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    public Transform top,buttom;
    public float speed;
    private float topy,buttomy;
    public bool isup=true;
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        coll=GetComponent<Collider2D>();
        topy=top.position.y;
        buttomy=buttom.position.y;
        Destroy(top.gameObject);
        Destroy(buttom.gameObject);
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if(isup)
        {
            rb.velocity=new Vector2(rb.velocity.x,speed);
            if(transform.position.y>topy)
            {
                isup=false;
            }
        }
        else
        {
            rb.velocity=new Vector2(rb.velocity.x,-speed);
            if(transform.position.y<buttomy)
            {
                isup=true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="bullet")
        {
            Destroy(gameObject);
        }
    }
}
