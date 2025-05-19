using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class MuTouRen : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Transform player;

    private float distance;

    // 发射频率
    public float fireRate = 1f;

    // 子弹速度
    public float bulletSpeed = 10f;

    private float nextFireTime = 0f;
    
    Animator animator;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);
        // 检测是否可以发射
        if (Time.time >= nextFireTime && player != null && distance <= 30f)
        {
            animator.SetTrigger("attack");
            StartCoroutine(ShootAtPlayer());
            nextFireTime = Time.time + Random.Range(0f, 3f) + fireRate;
        }
    }

    private IEnumerator ShootAtPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        // 获取夹角
        Vector2 direction = (player.transform.position - transform.position).normalized;
        GameObject bullet = BulletPool.instance.GetBullet();

        bullet.transform.position = transform.position + transform.up*3;
        //计算角度
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 随机角度
        float randomOffset = Random.Range(-90f, 90f);
        float finalAngle = angle + randomOffset;

        bullet.transform.rotation = Quaternion.AngleAxis(finalAngle, Vector3.forward);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        bulletSpeed = Random.Range(1f, 10f);
        rb.velocity = bullet.transform.right * bulletSpeed;
        
        StartCoroutine(BackPool(bullet));
        
    }



    IEnumerator BackPool(GameObject bullet)
    {
        yield return new WaitForSeconds(3f);
        BulletPool.instance.ReturnBullet(bullet);
    }
    

}