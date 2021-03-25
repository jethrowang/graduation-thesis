using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    public bool facing_right=true;
    public float speed;
    public float jumpforce;
    public float bumpforce;
    public LayerMask ground;
    public int poop;
    public Text poopnum;
    public bool ishurt;//默認false
    public GameObject bullet;
    public Transform firepoint;
    public float firerate; //firerate秒實例化一個子彈
    public float nextfire;
    public int hp;
    public int max_hp;
    public Image hp_bar;
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        hp = max_hp;
    }

    void FixedUpdate()
    {
        if(!ishurt)
        {
            Movement();
            Throw();
        }
        SwitchAnim();
        Hpfunction();
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
        if(facedirection>0&&!facing_right)
        {
            Flip();
        }else if(facedirection<0&&facing_right)
        {
            Flip();
        }
        // if(facedirection!=0)
        // {
        //     transform.localScale=new Vector3(facedirection*transform.localScale.x,transform.localScale.y,transform.localScale.z);
        //     transform.localScale=new Vector3(facedirection*0.2f,0.2f,0.2f);
            
        // }
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

    //翻轉
    void Flip()
    {
        facing_right=!facing_right;
        transform.Rotate(0f, 180f, 0f);
    }
    
    //切換動畫
    void SwitchAnim()
    {
        anim.SetBool("idle",false);

        if(rb.velocity.y<0.1f&&!coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling",true);
        }
        if(anim.GetBool("jumping"))
        {
            if(rb.velocity.y<0)
            {
                anim.SetBool("jumping",false);
                anim.SetBool("falling",true);
            }
        }else if(ishurt)
        {
            anim.SetBool("hurt",true);
            anim.SetFloat("running",0);
            if(Mathf.Abs(rb.velocity.x)<0.1f)
            {
                anim.SetBool("hurt",false);
                anim.SetBool("idle",true);
                ishurt=false;
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
        if(collision.gameObject.tag=="saliva")
        {
            Hurt();
        }
    }

    //消滅敵人
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="enemy")
        {
            virus virus=collision.gameObject.GetComponent<virus>();
            if(anim.GetBool("falling"))
            {
                virus.JumpOn();
                rb.velocity=new Vector2(rb.velocity.x,bumpforce*Time.deltaTime);
                anim.SetBool("jumping",true);
            }else if(transform.position.x<collision.gameObject.transform.position.x)
            {
                rb.velocity=new Vector2(-3,rb.velocity.y);
                ishurt=true;
            }else if(transform.position.x>collision.gameObject.transform.position.x)
            {
                rb.velocity=new Vector2(3,rb.velocity.y);
                ishurt=true;
            }
        }
        if(collision.gameObject.tag=="enemy")
        {
            Hurt();
        }
    }

    //受傷
    void Hurt()
    {
        hp -= 1;
        // flashcolor(flashtime);
        // Instantiate(blood,transform.position,Quaternion.identity);
    }

    //血量
    void Hpfunction()
    {
        if (hp <= 0)
        {
            Destroy(this.gameObject, 0.2f);
        }
        hp_bar.transform.localScale = new Vector2((float)hp / (float)max_hp, hp_bar.transform.localScale.y);
    }

    //射擊
    public void Throw()
    {
        if (Input.GetButtonDown("Throw"))
        {
            anim.SetTrigger("throw");
            if(Time.time>nextfire){ //讓子彈發射有間隔
                nextfire = Time.time + firerate; //Time.time表示從遊戲開發到現在的時間，會隨着遊戲的暫停而停止計算
                Invoke("bulletinstantiate",0.5f);
            }
        }
    }

    //子彈生成
    void bulletinstantiate(){
        Instantiate(bullet, firepoint.transform.position, firepoint.rotation);
    }
}
