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
    // 控制难度
    public int gameDifficulty; 

    public void ResetState()
    {
        
        int currentlevel = PlayerControl.level;
        if (currentlevel>=10)
        {
            currentlevel = 10;
        }
        gameDifficulty = currentlevel * 3;
        
        int randomInt = Random.Range(0, 100);
        if (randomInt < gameDifficulty + 20)
        {
            piano.SetActive(true);
        }
        else
        {
            piano.SetActive(false);
        }

        int randomInt2 = Random.Range(0, 100);
        if (randomInt2 < gameDifficulty*2 + 20)
        {
            muTouRen.SetActive(true);
        }
        else
        {
            muTouRen.SetActive(false);
        }

        int randomInt3 = Random.Range(0, 100);
        if (randomInt3 < gameDifficulty/2 +10)
        {
            floorOba.SetActive(true);
        }
        else
        {
            floorOba.SetActive(false);
        }
        
    }


    public void CloseAll()
    {
        piano.SetActive(false);
        floorOba.SetActive(false);
        muTouRen.SetActive(false);

    }
}