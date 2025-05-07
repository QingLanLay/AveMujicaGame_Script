using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriver : MonoBehaviour 
{
    public GameObject car;
    private Rigidbody2D rb;
    private GameObject player;
    private float smoothTime = 1f;
    
    private Vector3 carVelocity = Vector3.zero; // 由SmoothDamp内部使用
    private void Start()
    { 
        rb = car.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        rb.velocity = Vector2.right * 5f;
        
        if (car.transform.position.x < player.transform.position.x - 50f)
        {
            car.transform.position = new Vector3(
                player.transform.position.x - 50f,
                car.transform.position.y,
                car.transform.position.z
            );
        }
    }
}
