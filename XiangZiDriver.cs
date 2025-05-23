using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XiangZiDriver : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    private Rigidbody2D playerRb;
    private Animator animator;
    private float smoothTime = 0.1f;
    private Vector2 currentVelocity;
    private Vector2 targetVelocity;
    private bool run;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerRb = player.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }


    private void FixedUpdate()
    {
       

        if (PlayerControl.level > 10)
        {
            // 如果距离玩家小于10
            if (rb.transform.position.x  - player.transform.position.x < 10f )
            {
                targetVelocity = new Vector2(playerRb.velocity.x,0);
                // 在FixedUpdate中平滑调整速度
                rb.velocity = Vector2.SmoothDamp(rb.velocity, new Vector2(15f,0), 
                    ref currentVelocity, smoothTime);
    
                // 使用MovePosition确保物理系统正确更新
                rb.MovePosition(rb.position + rb.velocity * Time.fixedDeltaTime);
            
                // 动画
                run = true;
                animator.SetBool("run",run);
            
            }
            else
            {
                rb.velocity = Vector2.SmoothDamp(rb.velocity, new Vector2(5f,0), 
                    ref currentVelocity, smoothTime);
                run = false;
                animator.SetBool("run",run);

            }
        }
        else
        {
            levelLow10Speed();
        }
    }

    private void levelLow10Speed()
    {
        // 如果距离玩家小于10
        if (rb.transform.position.x  - player.transform.position.x < 10f )
        {
            targetVelocity = new Vector2(playerRb.velocity.x,0);
            // 在FixedUpdate中平滑调整速度
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, 
                ref currentVelocity, smoothTime);
    
            // 使用MovePosition确保物理系统正确更新
            rb.MovePosition(rb.position + rb.velocity * Time.fixedDeltaTime);
            
            // 动画
            run = true;
            animator.SetBool("run",run);
            
        }
        else
        {
            rb.velocity = Vector2.SmoothDamp(rb.velocity, new Vector2(5f,0), 
                ref currentVelocity, smoothTime);
            run = false;
            animator.SetBool("run",run);

        }

        // 如果速度小于5，则速度等于5
        if (rb.velocity.x < 5f)
        {
            rb.velocity = new Vector2(5f, rb.velocity.y);

        }
    }
}
