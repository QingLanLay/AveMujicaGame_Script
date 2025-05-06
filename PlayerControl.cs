using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        // 刚体
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 地面检测
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.6f);
        
        // 跳跃输入
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // 清除当前垂直速度后再施加跳跃力
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        // 水平移动
        float horizontal = Input.GetAxis("Horizontal");
        rb.AddForce(new Vector2(horizontal * speedForce, 0));
        
        // 速度最大值控制
        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }

        if (rb.velocity.x < -maxSpeed)
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
}