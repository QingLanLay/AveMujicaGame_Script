using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class RandomTransform : MonoBehaviour 
{


    private void OnEnable()
    {
        float x = transform.parent.position.x + (UnityEngine.Random.Range(-15, 15));
        transform.position =new Vector3(x,transform.position.y,transform.position.z) ;

        if (this.tag=="MuTouRen")
        {
            float y = transform.parent.position.y + 6.82f + (UnityEngine.Random.Range(0, 3));
            transform.position =new Vector3(transform.position.x,y,transform.position.z ) ;
        }
    }
}
