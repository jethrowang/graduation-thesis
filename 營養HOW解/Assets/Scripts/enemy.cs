using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform leftpoint,rightpoint;
    private bool faceleft=true;
    public float speed;
    private float leftx,rightx;
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        leftx=leftpoint.position.x;
        rightx=rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    void Update()
    {
        movement();
    }

    void movement()
    {
        if(faceleft)
        {
            rb.velocity=new Vector2(-speed,rb.velocity.y);
            if(transform.position.x<leftx)
            {
                transform.localScale=new Vector3(-0.12f,0.12f,0.12f);
                faceleft=false;
            }
        }else
        {
            rb.velocity=new Vector2(speed,rb.velocity.y);
            if(transform.position.x>rightx)
            {
                transform.localScale=new Vector3(0.12f,0.12f,0.12f);
                faceleft=true;
            }
        }
    }
}
