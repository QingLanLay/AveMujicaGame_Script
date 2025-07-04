using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Foot : MonoBehaviour 
{
    GameObject player;
    public  GameObject xiangZi;
    private Rigidbody2D rb;
    
    private void Start()
    {
         player = GameObject.FindWithTag("Player");
         rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        AudioPool.instance.PlayGameMusic("saki成神");
        transform.position =  xiangZi.transform.position+Vector3.up* 3+ Vector3.right*3;
    }
    
    private void FixedUpdate() {
        
        if ( player.transform.position.x -
            transform.position.x >25f)
        {
            gameObject.SetActive(false);
        }
        rb.velocity = new Vector2(-15f, 0);
    }
}
