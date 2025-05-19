using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hand : MonoBehaviour 
{
    GameObject player;
    public  GameObject car;
    private Rigidbody2D rb;
    
    private void Start()
    {
         player = GameObject.FindWithTag("Player");
         rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        transform.position =  car.transform.position;
    }
    
    private void FixedUpdate() {
        if (transform.position.x - player.transform.position.x >25f)
        {
            gameObject.SetActive(false);
        }
        rb.velocity = new Vector2(20f, 0);
    }
}
