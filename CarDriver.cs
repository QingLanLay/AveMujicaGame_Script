using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class CarDriver : MonoBehaviour
{
    public GameObject car;
    private Rigidbody2D rb;
    private GameObject player;
    private float smoothTime = 1f;

    private Vector3 carVelocity = Vector3.zero; // 由SmoothDamp内部使用
    public GameObject hand;
    
    // 计时器
    public float interval = 50f;
    
    // 计时器是否正在运行
    public bool isRunning = false;
    
    // 计时器累计时间
    public float elapsedTime = 0f;
    

    private void Start()
    {
        rb = car.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartTimer();
    }

    private void Update()
    {
        rb.velocity = Vector2.right * 5f;

        if (car.transform.position.x < player.transform.position.x - 25f)
        {
            car.transform.position = new Vector3(
                player.transform.position.x - 25f,
                car.transform.position.y,
                car.transform.position.z
            );
        }

        if (car.transform.transform.position.x > player.transform.position.x)
        {
            StartCoroutine(Reborn());
        }

        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            
            if (elapsedTime >= interval/PlayerControl.level)
            {
                if (hand.activeSelf == false )
                {
                    handGo();
                    // 重置计时器
                    elapsedTime = 0f;
                }
            
            }
        }
        
    }

    private void handGo()
    {
        hand.SetActive(true);
    }

    private IEnumerator Reborn()
    {
        yield return new WaitForSeconds(1f);
        player.transform.position = new Vector3(car.transform.position.x + 10f, 5f, 0);
    }
    
    // 启动计时器
    public void StartTimer()
    {
        isRunning = true;
        elapsedTime = 0f;
    }

    // 停止计时器
    public void StopTimer()
    {
        isRunning = false;
    }
}