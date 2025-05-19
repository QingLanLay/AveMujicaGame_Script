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
    public GameObject hand;

    private void Start()
    {
        rb = car.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
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

        if (hand.activeSelf == false)
        {
            StartCoroutine(handGo());
        }
    }

    private IEnumerator handGo()
    {
        yield return new WaitForSeconds(2f);
        hand.SetActive(true);
    }

    private IEnumerator Reborn()
    {
        yield return new WaitForSeconds(1f);
        player.transform.position = new Vector3(car.transform.position.x+10f, 5f,0);
    }
}