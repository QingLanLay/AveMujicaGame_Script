using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




public class RandomTransform : MonoBehaviour 
{
    private void OnEnable()
    {
        transform.position = transform.position + (UnityEngine.Random.Range(-15, 15) * Vector3.right);
    }
}
