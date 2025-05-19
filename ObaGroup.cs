using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ObaGroup : MonoBehaviour
{
    public GameObject piano;
    public GameObject floorOba;
    public GameObject muTouRen;
    public GameObject hand;

    public void ResetState()
    {
        int randomInt = Random.Range(0, 100);
        if (randomInt > 49)
        {
            piano.SetActive(true);
        }
        else
        {
            piano.SetActive(false);
        }

        int randomInt2 = Random.Range(0, 100);
        if (randomInt2 > 50)
        {
            muTouRen.SetActive(true);
        }
        else
        {
            muTouRen.SetActive(false);
        }

        int randomInt3 = Random.Range(0, 100);
        if (randomInt3 > 50)
        {
            floorOba.SetActive(true);
        }
        else
        {
            floorOba.SetActive(false);
        }

        int randomInt4 = Random.Range(0, 100);
        if (randomInt4 > 50)
        {
            hand.SetActive(true);
        }
        else
        {
            hand.SetActive(false);
        }
    }


    public void CloseAll()
    {
        piano.SetActive(false);
        floorOba.SetActive(false);
        muTouRen.SetActive(false);
        hand.SetActive(false);
    }
}