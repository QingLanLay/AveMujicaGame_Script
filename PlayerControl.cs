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
    int health = 0;

    public float speedForce = 15f;
    public float maxSpeed = 15f;

    public float jumpForce = 10f;

    // 增加下落重力
    public float gravityMultiplier = 3f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;
    public LayerMask groundLayer;

    // Trigger检测中点
    // 初始化
    public GameObject startGround;

    // 当前所在Ground
    private GameObject currentGround;

    // 上一个Ground
    private GameObject previousGround;

    // 击退判定
    private bool isKnockback = false;

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

        // 跳跃输入
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // 清除当前垂直速度后再施加跳跃力
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            animator.SetTrigger("jump");
        }

        float currentSpeed = rb.velocity.x;
        animator.SetFloat("speed", Mathf.Abs(currentSpeed));
    }

    void FixedUpdate()
    {
        // 水平移动
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (vertical >= 0f)
        {
            animator.SetBool("crouch", false);
        }
        else if (vertical < 0f)
        {
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

        // 速度最大值控制
        if (rb.velocity.x > maxSpeed && !isKnockback)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
        
        if (rb.velocity.x < -maxSpeed && !isKnockback )
        {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        }

        // 下落
        if (transform.position.y > -3.095233f)
        {
            // 使用较小的持续力
            rb.AddForce(Vector2.down * 20f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检测是否触碰到中点
        if (other.CompareTag("MidTrigger"))
        {
            // 获取当前地板
            GameObject nowGround = other.gameObject.transform.parent.gameObject;


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
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        
        if (other.gameObject.CompareTag("Car") )
        {
            // 进入击退状态
            isKnockback = true;
            speedForce = 30f;   

            animator.SetTrigger("damage");
            // 清零当前速度（仅保留垂直速度，防止影响跳跃）
            rb.velocity = new Vector2(0f, rb.velocity.y);
            rb.AddForce(Vector2.right * 20f, ForceMode2D.Impulse);
            
            
            // 协程控制时间
            StartCoroutine(ResetKnockback());
        }else if ( other.gameObject.CompareTag("Obstacles"))
        {
            // 进入击退状态
            isKnockback = true;
            speedForce = 30f;   
            animator.SetTrigger("damage");

            // 判断相对位置
            Vector3 relativePositon = other.gameObject.transform.position - transform.position;
            if (relativePositon.x >= 0 )
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
        
        if (other.gameObject.CompareTag("Obstacles"))
        {
            // 短暂忽略碰撞（0.2秒）
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.collider, true);
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
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), otherCollider, false);
 
    }
}