using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
    private void OnEnable()
    {
        StartCoroutine(BulletCoroutine());
    }

    private IEnumerator BulletCoroutine()
    {
        yield return new WaitForSeconds(5f);
        BulletPool.instance.ReturnBullet(gameObject);
    }
}
