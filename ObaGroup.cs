using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ObaGroup : MonoBehaviour
{
    public GameObject piano;
    public GameObject floorOba;

    public void ResetState()
    {

        
        int randomInt = Random.Range(0, 100);
        if (randomInt > 49)
        {
            piano.SetActive(true) ;
            floorOba.SetActive(false);
        }
        else
        {
            piano.SetActive(false) ;
            floorOba.SetActive(true);
        }

    }
    
    
    
}
