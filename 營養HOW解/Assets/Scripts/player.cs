using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    [Header("狀態")]
    public bool facing_right=true;
    public bool ishurt;
    public bool iscrouch;
    public bool isGround;
    public bool isjump;
    public bool jumpPressed,jumpHeld,crouchHeld;
    [Header("環境")]
    public Transform ceilingCheck,groundCheck;
    public LayerMask ground;
    [Header("碰撞器")]
    public Collider2D coll;
    public Collider2D discoll;
    [Header("移動")]
    public float speed;
    public float crouchspeedDivisor;
    [Header("跳躍")]
    public float jumpforce;
    public float jumpholdforce;
    public float jumptime;
    public float jumpholdduration;
    public float crouchjumpboost;
    public float bumpforce;
    public int extraJump;
    [Header("收集")]
    public int poop;
    public Text poopnum;
    [Header("子彈")]
    public GameObject bullet;
    public Transform firepoint;
    public float firerate; //firerate秒實例化一個子彈
    public float nextfire;
    [Header("血條")]
    public int hp;
    public int max_hp;
    public Image hp_bar;
    [Header("搖桿")]
    public Joystick joystick;
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
        isGround=Physics2D.OverlapCircle(groundCheck.position,0.2f,ground);
    }

    void Update()
    {
        jumpPressed=Input.GetButtonDown("Jump");
        jumpHeld=Input.GetButton("Jump");
        crouchHeld=Input.GetButton("Crouch");
        Jump2();
        Crouch();
        Poopnum();
    }

    //移動
    void Movement()
    {
        float horizontalmove=Input.GetAxis("Horizontal");
        float facedirection=Input.GetAxisRaw("Horizontal");
        //移動
        if(horizontalmove!=0)
        {
            if(iscrouch)
            {
                horizontalmove/=crouchspeedDivisor;
                rb.velocity=new Vector2(horizontalmove*speed*Time.fixedDeltaTime,rb.velocity.y);
                anim.SetFloat("running",Mathf.Abs(facedirection));
            }else
            {
                rb.velocity=new Vector2(horizontalmove*speed*Time.fixedDeltaTime,rb.velocity.y);
                anim.SetFloat("running",Mathf.Abs(facedirection));
            }
        }
        //方向
        if(facedirection>0f&&!facing_right)
        {
            Flip();
        }else if(facedirection<0f&&facing_right)
        {
            Flip();
        }
    }

    //蹲下
    void Crouch()
    {
        if(!Physics2D.OverlapCircle(ceilingCheck.position,0.2f,ground))
        {
            if(Input.GetButton("Crouch"))
            {
                iscrouch=true;
                anim.SetBool("crouching",true);
                discoll.enabled=false;
            }else
            {
                iscrouch=false;
                discoll.enabled=true;
                anim.SetBool("crouching",false);
            }
        }
    }

    //跳躍(二段)
    void Jump1()
    {
        if(isGround)
        {
            extraJump=1;
        }
        if(Input.GetButtonDown("Jump")&&extraJump>0)
        {
            rb.velocity=Vector2.up*jumpforce;
            extraJump--;
            soundmanager.instance.Jumpaudio();
            anim.SetBool("jumping",true);
        }
        if(Input.GetButtonDown("Jump")&&extraJump==0&&isGround)
        {
            rb.velocity=Vector2.up*jumpforce;
            soundmanager.instance.Jumpaudio();
            anim.SetBool("jumping",true);
        }
    }

    //跳躍(長按)
    void Jump2()
    {
        if(jumpPressed&&isGround&&!isjump)
        {
            if(iscrouch&&isGround)
            {
                soundmanager.instance.Jumpaudio();
                anim.SetBool("jumping",true);
                rb.AddForce(new Vector2(0f,crouchjumpboost),ForceMode2D.Impulse);
            }
            isGround=false;
            isjump=true;
            jumptime=Time.time+jumpholdduration;
            soundmanager.instance.Jumpaudio();
            anim.SetBool("jumping",true);
            rb.AddForce(new Vector2(0f,jumpforce),ForceMode2D.Impulse);
        }else if(isjump)
        {
            if(jumpHeld)
            {
                rb.AddForce(new Vector2(0f,jumpholdforce),ForceMode2D.Impulse);
            }
            if(jumptime<Time.time)
            {
                isjump=false;
            }
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
                ishurt=false;
            }
        }else if(coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling",false);
        }
    }

    //碰撞觸發
    void OnTriggerEnter2D(Collider2D collision)
    {
        //收集物品
        if(collision.tag=="collection")
        {
            soundmanager.instance.Poopaudio();
            collision.GetComponent<Animator>().Play("collect");
        }
        //受傷
        if(collision.tag=="saliva")
        {
            ishurt=true;
            Hurt();
        }
        //重新開始
        if(collision.tag=="deadline")
        {
            // GetComponent<AudioSource>().enabled=false;
            Invoke("Restart",2f);
        }
    }

    //消滅敵人
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="enemy")
        {
            enemy enemy=collision.gameObject.GetComponent<enemy>();
            if(anim.GetBool("falling"))
            {
                enemy.JumpOn();
                rb.velocity=new Vector2(rb.velocity.x,bumpforce*Time.deltaTime);
                anim.SetBool("jumping",true);
            }else if(transform.position.x<collision.gameObject.transform.position.x)
            {
                rb.velocity=new Vector2(-3,rb.velocity.y);
                ishurt=true;
                Hurt();
            }else if(transform.position.x>collision.gameObject.transform.position.x)
            {
                rb.velocity=new Vector2(3,rb.velocity.y);
                ishurt=true;
                Hurt();
            }
        }
    }

    //受傷
    void Hurt()
    {
        soundmanager.instance.Hurtaudio();
        hp -= 1;
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
            if(Time.time>nextfire) //讓子彈發射有間隔
            {
                anim.SetTrigger("throw");
                nextfire = Time.time + firerate; //Time.time表示從遊戲開發到現在的時間，會隨着遊戲的暫停而停止計算
                Invoke("Bulletinstantiate",0.5f);
                anim.SetBool("idle",true);
            }
        }
    }

    //子彈生成
    void Bulletinstantiate()
    {
        Instantiate(bullet, firepoint.transform.position, firepoint.rotation);
    }

    //重新開始
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //收集計算
    public void Poopcount()
    {
        poop+=1;
    }

    //收集計數
    void Poopnum()
    {
        poopnum.text="X"+poop.ToString();
    }
}
