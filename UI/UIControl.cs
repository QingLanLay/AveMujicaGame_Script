using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public PlayerControl playerControl;
    public Text wudiNum;
    public Text levelNum;
    public Text health;
    public Text time;
    public GameObject menuUI;
    public GameObject endUI;
    private void Update()
    {
        wudiNum.text = "无敌次数:" + playerControl.wudiNum;
        levelNum.text = "Level:" + PlayerControl.level;
        health.text = "Health:" + PlayerControl.health;
      
        int minutes = (int)(PlayerControl.gameTime / 60f);
        int seconds = (int)(PlayerControl.gameTime % 60f);
        time.text = "Time:"+string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    public void StopTimeOpenMenu()
    {
        menuUI.SetActive(true);
        Time.timeScale = 0;
    }
}