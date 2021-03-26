using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class virus : enemy
{
    private Rigidbody2D rb;
    // private Animator anim;
    private Collider2D coll;
    public LayerMask ground;
    public Transform leftpoint,rightpoint;
    public bool faceleft=true;
    public float speed,jumpforce;
    private float leftx,rightx;
    protected override void Start()
    {
        base.Start();
        rb=GetComponent<Rigidbody2D>();
        // anim=GetComponent<Animator>();
        coll=GetComponent<Collider2D>();
        leftx=leftpoint.position.x;
        rightx=rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    void Update()
    {
        SwitchAnim();
    }

    void Movement()
    {
        if(faceleft)//面左
        {
            if(coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping",true);
                rb.velocity=new Vector2(-speed,jumpforce);
            }
            if(transform.position.x<leftx)
            {
                transform.localScale=new Vector3(-0.5f,0.5f,0.5f);
                faceleft=false;
            }
        }else//面右
        {
            if(coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping",true);
                rb.velocity=new Vector2(speed,jumpforce);
            }
            if(transform.position.x>rightx)
            {
                transform.localScale=new Vector3(0.5f,0.5f,0.5f);
                faceleft=true;
            }
        }
    }

    void SwitchAnim()
    {
        if(anim.GetBool("jumping"))
        {
            if(rb.velocity.y<0.1)
            {
                anim.SetBool("jumping",false);
                anim.SetBool("falling",true);
            }
        }
        if(coll.IsTouchingLayers(ground)&&anim.GetBool("falling"))
        {
            anim.SetBool("falling",false);
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
