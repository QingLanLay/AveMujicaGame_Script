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
    
    public Slider musicSlider;
    public Slider ambientSlider;
    public static UIControl instance;

    public void Awake()
    {
        instance = this;
    }

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

    public void ChangeAmbient()
    {
        AudioManager.instance.mixer.SetFloat("Ambient", AudioManager.instance.ConertSoundVolume(ambientSlider.value));
    }

    public void ChangeMusic()
    {
        AudioManager.instance.mixer.SetFloat("Music", AudioManager.instance.ConertSoundVolume(musicSlider.value));

    }
}