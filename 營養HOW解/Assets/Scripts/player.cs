using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    public int poop;
    public Text poopnum;
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
        if(Input.GetButtonDown("Jump")&&coll.IsTouchingLayers(ground))
        {
            rb.velocity=new Vector2(rb.velocity.x,jumpforce*Time.deltaTime);
            anim.SetBool("jumping",true);
        }
        //蹲下
        if(Input.GetButtonDown("Crouch")&&coll.IsTouchingLayers(ground))
        {
            anim.SetBool("idle",false);
            anim.SetBool("crouching",true);
        }
        if(Input.GetButtonUp("Crouch")&&coll.IsTouchingLayers(ground))
        {
            anim.SetBool("crouching",true);
            anim.SetBool("idle",true);
        }
    }
    
    //切換動畫
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

    //收集物品
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="collection")
        {
            Destroy(collision.gameObject);
            poop+=1;
            poopnum.text="X"+poop.ToString();
        }
    }

    //消滅敵人
    void OnCollisionEnter2D(Collider2D collision)
    {
        if(anim.GetBool("falling"))
        {
            if(collision.gameObject.tag=="enemy")
            {
            Destroy(collision.gameObject);
            rb.velocity=new Vector2(rb.velocity.x,jumpforce*Time.deltaTime);
            anim.SetBool("jumping",true);
            }
        }
    }
}
