using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class virus : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    public LayerMask ground;
    public Transform leftpoint,rightpoint;
    private bool faceleft=true;
    public float speed,attackforce;
    private float leftx,rightx;
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
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
                anim.SetBool("attacking",true);
                rb.velocity=new Vector2(-speed,attackforce);
            }
            if(transform.position.x<leftx)
            {
                transform.localScale=new Vector3(-0.7f,0.7f,0.7f);
                faceleft=false;
            }
        }else//面右
        {
            if(coll.IsTouchingLayers(ground))
            {
                anim.SetBool("attacking",true);
                rb.velocity=new Vector2(speed,attackforce);
            }
            if(transform.position.x>rightx)
            {
                transform.localScale=new Vector3(0.7f,0.7f,0.7f);
                faceleft=true;
            }
        }
    }

    void SwitchAnim()
    {
        if(coll.IsTouchingLayers(ground)&&anim.GetBool("attacking"))
        {
            anim.SetBool("attacking",false);
        }
    }

    void Death()
    {
        anim.SetTrigger("death");
    }

    public void JumpOn()
    {
        Destroy(gameObject);
    }
}
