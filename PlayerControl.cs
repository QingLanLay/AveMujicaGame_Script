using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    // 生命值
    public static int health = 10;

    public float speedForce = 15f;
    public float realSpeed = 15f;
    public float maxSpeed = 15f;
    public float jumpForce = 10f;

    // 增加下落重力
    public float gravityMultiplier = 3f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;
    public LayerMask groundLayer;
    bool isCrouch;

    // Trigger检测中点
    // 初始化
    public GameObject startGround;
    public GameObject nowGround;
    private bool inMid;

    // 当前所在Ground
    public GameObject currentGround;

    // 上一个Ground
    public GameObject previousGround;

    // 击退判定
    private bool isKnockback = false;
    private bool inPiano;

    // 在钢琴上
    // public bool onPinao  = false;
    // 在大手里
    bool inHand = false;
    
    // level
    public static int level = 0;
    // 当前anon奖励吃没吃
    public  bool isUp = false;
    
    // 无敌次数
    public int wudiNum = 2;
    private bool isWudi = false;


    // time
    public static float gameTime;

    public GameObject foot;
    void Start()
    {
        // 刚体
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // 初始化Ground
        previousGround = startGround;
    }

    void Update()
    {
        // 地面检测
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, groundLayer);
        animator.SetBool("isGround", isGrounded);

        // 速度控制
        if (!inHand)
        {
            if (level >= 10  && !isCrouch )
            {
                maxSpeed = realSpeed + 10;
            }
            else if (level < 10 && !isCrouch  )
            {
                maxSpeed = realSpeed + level;
            }else if (level >= 10 && isCrouch)
            {
                maxSpeed = (realSpeed + 10)*0.85f;
            }else if (level < 10 && isCrouch )
            {
                maxSpeed = (realSpeed + level)*0.85f;
            }
            
        }

        // 跳跃输入
        if (Input.GetKeyDown(KeyCode.Space)&& isGrounded)
        {
            GetJump();
        }

        // 动画切换
        float currentSpeed = rb.velocity.x;
        animator.SetFloat("speed", Mathf.Abs(currentSpeed));

        if (Input.GetKeyDown(KeyCode.J) && wudiNum > 0 && !isWudi) 
        {
            StartCoroutine(IngoneBox());
        }

        currentGameTime();
        
    }

    private void currentGameTime()
    {
        gameTime += Time.deltaTime;
    }

    private IEnumerator IngoneBox()
    {
        var wudi = this.transform.GetChild(0);
        wudi.gameObject.SetActive(true);
        this.gameObject.layer = LayerMask.NameToLayer("Wudi");
        wudiNum = wudiNum - 1;
        isWudi = true;
        yield return new WaitForSeconds(2f);
        wudi.gameObject.SetActive(false);
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        isWudi = false;
        Invoke(nameof(ReWudi),10f);
    }

    void ReWudi()
    {
        if (wudiNum < 2 )
        {
            wudiNum += 1;
        }
    }

    private void GetJump()
    {
        // 清除当前垂直速度后再施加跳跃力
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        animator.SetTrigger("jump");
    }

    void FixedUpdate()
    {
        // 水平移动
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // 移动控制
        MoveControl(vertical, horizontal);

        // 速度最大值控制
        MaxSpeedControl();

        // 下落
        if (transform.position.y > -3.095233f)
        {
            // 使用较小的持续力
            rb.AddForce(Vector2.down * 20f);
        }
    }

    /// <summary>
    ///  最大速度控制
    /// </summary>
    private void MaxSpeedControl()
    {
        if (rb.velocity.x > maxSpeed && !isKnockback)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }

        if (rb.velocity.x < -maxSpeed && !isKnockback)
        {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        }
    }

    /// <summary>
    /// 移动控制
    /// </summary>
    /// <param name="vertical"></param>
    /// <param name="horizontal"></param>
    private void MoveControl(float vertical, float horizontal)
    {
        // 进钢琴下蹲
        if (vertical >= 0f && !inPiano)
        {
            isCrouch = false;
            animator.SetBool("crouch", false);
        }
        else if (vertical < 0f)
        {
            isCrouch = true;
            animator.SetBool("crouch", true);
        }

        rb.AddForce(new Vector2(horizontal * speedForce, 0));

        // 方向控制
        if (horizontal > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (horizontal < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检测是否触碰到中点
        if (other.CompareTag("MidTrigger") && !inMid)
        {
            inMid = true;
            // 获取当前地板
             nowGround = other.gameObject.transform.parent.gameObject;
            
            // 如果存在上一个Ground，则右移100
            if (previousGround != null && nowGround != previousGround)
            {
                previousGround.transform.position += Vector3.right * 150;
                previousGround.GetComponentInChildren<Obstacles>().ResetState();    
            }

            // 更新地板信息
            currentGround = nowGround;
            previousGround = currentGround;
        }

        if (other.CompareTag("Obstacles"))
        {
            // 进入击退状态
            isKnockback = true;
            speedForce = 30f;
            animator.SetTrigger("damage");
            
            health = health - 1;
            // 判断相对位置
            Vector3 relativePositon = other.gameObject.transform.position - transform.position;
            if (relativePositon.x >= 0)
            {
                // 清零当前速度（仅保留垂直速度，防止影响跳跃）
                rb.velocity = new Vector2(0f, rb.velocity.y);
                rb.AddForce(Vector2.left * 10f, ForceMode2D.Impulse);
            }
            else
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
                rb.AddForce(Vector2.right * 10f, ForceMode2D.Impulse);
            }

            StartCoroutine(ResetKnockback());
        }

        if (other.CompareTag("Piano"))
        {
            inPiano = true;
        }

        if (other.CompareTag("Hand"))
        {
            inHand = true;
            maxSpeed = 0f;
        }

        if (other.CompareTag("Anon"))
        {
            other.GetComponent<Animator>().SetBool("on",true);
            if (!isUp)
            {
                if (health<10)
                {
                    health = health + 1;
                }
                level += 1;
                isUp = true;
            }
        }

        if (other.CompareTag("XiangZi"))
        {
            foot.SetActive(true);
            inXiangzi = true;
           
            if (touchXiangzi > 1)
            {
                InitManager.instance.OpenEndUI();
            }
        }
    }
    
    public bool inXiangzi = false;
    public int touchXiangzi = 0;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MidTrigger"))
        {
            inMid = false;
        }
        
        if (other.CompareTag("Piano"))
        {
            inPiano = false;
        }

        if (other.CompareTag("Hand"))
        {
            maxSpeed = realSpeed;
            inHand = false;
        }

        if (other.CompareTag("Anon"))
        {
           
            if (Vector2.Distance(this.transform.position, other.transform.position) > 20f)
            {
                other.GetComponent<Animator>().SetBool("on",false);
            }
        }

        if (other.CompareTag("XiangZi"))
        {
            inXiangzi = false;    
            touchXiangzi +=1;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Car"))
        {
            health = health - 3;
            // 进入击退状态
            isKnockback = true;
            speedForce = 30f;

            animator.SetTrigger("damage");
            // 清零当前速度（仅保留垂直速度，防止影响跳跃）
            rb.velocity = new Vector2(0f, rb.velocity.y);
            rb.AddForce(Vector2.right * 20f, ForceMode2D.Impulse);


            // 协程控制时间
            StartCoroutine(ResetKnockback());
        }
        else if (other.gameObject.CompareTag("Obstacles"))
        {
            // 进入击退状态
            isKnockback = true;
            speedForce = 30f;
            animator.SetTrigger("damage");
            health = health - 1;

            // 判断相对位置
            Vector3 relativePositon = other.gameObject.transform.position - transform.position;
            if (relativePositon.x >= 0)
            {
                // 清零当前速度（仅保留垂直速度，防止影响跳跃）
                rb.velocity = new Vector2(0f, rb.velocity.y);
                rb.AddForce(Vector2.left * 10f, ForceMode2D.Impulse);
            }
            else
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
                rb.AddForce(Vector2.right * 10f, ForceMode2D.Impulse);
            }

            StartCoroutine(ResetKnockback());

            // 短暂忽略碰撞
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), other.collider, true);
            Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(), other.collider, true);

            StartCoroutine(ReEnableCollision(other.collider));
        }
    }


    // 碰撞时短暂放宽极限速度
    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(0.4f);
        isKnockback = false;
        speedForce = 100f;
    }


    // 关闭碰撞体防止bug
    private IEnumerator ReEnableCollision(Collider2D otherCollider)
    {
        yield return new WaitForSeconds(3f);
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), otherCollider, false);
        Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(), otherCollider, false);
    }
    

}