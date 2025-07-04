using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnonControl : MonoBehaviour
{
    Animator animator;
    GameObject player;
    PlayerControl playerControl;
    private bool isplay;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        playerControl = player.GetComponent<PlayerControl>();
    }

    private void OnEnable()
    {
        animator.SetBool("on", false);
        playerControl.isUp = false;
        isplay = false;
    }

    private void Update()
    {
        if ( Vector3.Distance(this.transform.position, player.transform.position)< 15f && !isplay )
        {
            AudioPool.instance.PlayGameMusic("anon丰川家族",0.5f);
            isplay = true;
        }
    }

    private void OnDisable()
    {
        playerControl.isUp = false;
    }

    public void ResetState()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}