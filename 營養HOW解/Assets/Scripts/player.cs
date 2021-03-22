using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    public int poop;
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Movement();
        SwitchAnim();
    }

    void Movement()
    {
        float horizontalmove=Input.GetAxis("Horizontal");
        float facedirection=Input.GetAxisRaw("Horizontal");
        //移動
        if(horizontalmove!=0)
        {
            rb.velocity=new Vector2(horizontalmove*speed*Time.deltaTime,rb.velocity.y);
            anim.SetFloat("running",Mathf.Abs(facedirection));
        }
        //方向
        if(facedirection!=0)
        {
            // transform.localScale=new Vector3(facedirection*transform.localScale.x,transform.localScale.y,transform.localScale.z);
            transform.localScale=new Vector3(facedirection*0.2f,0.2f,0.2f);
        }
        //跳躍
        if(Input.GetButtonDown("Jump"))
        {
            rb.velocity=new Vector2(rb.velocity.x,jumpforce*Time.deltaTime);
            anim.SetBool("jumping",true);
        }
    }

    void SwitchAnim()
    {
        anim.SetBool("idle",false);

        if(anim.GetBool("jumping"))
        {
            if(rb.velocity.y<0)
            {
                anim.SetBool("jumping",false);
                anim.SetBool("falling",true);
            }
        }else if(coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling",false);
            anim.SetBool("idle",true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="collection")
        {
            Destroy(collision.gameObject);
            poop=poop+1;
        }
    }
}
