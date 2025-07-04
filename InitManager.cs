using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitManager : MonoBehaviour
{
    public GameObject xiangZi;
    public GameObject player;
    public GameObject car;
    public GameObject hand;
    public GameObject floorBase1;
    public GameObject floorBase2;
    public GameObject floorBase3;
    public GameObject menuUI;
    public GameObject carDriver;

    public GameObject endUI;
    public Text endText;
    public GameObject foot;

    public float maxCount = 0f;
    public Text maxCountText;
    
    public GameObject startUI; 
    

    Vector3 initXiangZi;
    Vector3 initPlayer;
    Vector3 initCar;
    Vector3 initHand;
    Vector3 initFloorBase1;
    Vector3 initFloorBase2;
    Vector3 initFloorBase3;

    public static InitManager instance;

    public GameObject endImage1;
    public GameObject endImage2;
    public bool startGame = false;
    
    [Header("移动端")]
    public GameObject isJump;
    public GameObject isWudi;
    public GameObject JoyStick;
    private void Awake()
    {
                
        isJump.SetActive(false);
        isWudi.SetActive(false);
        JoyStick.SetActive(false);
        if (instance == null)
        {
            instance = this;
        }

        
        initXiangZi = xiangZi.transform.position;
        initPlayer = player.transform.position;
        initCar = car.transform.position;
        initHand = hand.transform.position;
        initFloorBase1 = floorBase1.transform.position;
        initFloorBase2 = floorBase2.transform.position;
        initFloorBase3 = floorBase3.transform.position;
        carDriver = GameObject.FindGameObjectWithTag("Car");

        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (PlayerControl.health <= 0)
        {
            OpenEndUI();
        }
    }

    public void ReStart()
    {
#if UNITY_ANDROID
        isJump.SetActive(true);
        isWudi.SetActive(true);
        JoyStick.SetActive(true);
#endif

        PlayerControl.beHurted = 0;
        PlayerControl.useWudiNum = 0;
        StopAllCoroutines();
        menuUI.SetActive(false);
        Time.timeScale = 1;
        xiangZi.transform.position = initXiangZi;
        hand.SetActive(false);
        
        player.transform.position = initPlayer;
        

        player.GetComponent<PlayerControl>().startGround = floorBase1;
        player.GetComponent<PlayerControl>().currentGround = null;
        player.GetComponent<PlayerControl>().nowGround = null;
        player.GetComponent<PlayerControl>().previousGround = floorBase1;
        foot.SetActive(false);
        player.GetComponent<PlayerControl>().touchXiangzi = 0;

        PlayerControl.gameTime = 0;
        PlayerControl.health = 10;
        PlayerControl.level = 0;

        car.transform.position = initCar;

        hand.transform.position = initHand;
        hand.SetActive(false);


        floorBase1.transform.position = initFloorBase1;
        floorBase2.transform.position = initFloorBase2;
        floorBase3.transform.position = initFloorBase3;

        floorBase1.GetComponentInChildren<Obstacles>().ResetState();
        floorBase2.GetComponentInChildren<Obstacles>().ResetState();
        floorBase3.GetComponentInChildren<Obstacles>().ResetState();
        
        carDriver.GetComponent<CarDriver>().StartTimer();
        endUI.SetActive(false);
    }

    public void Back()
    {
        Time.timeScale = 1;
        menuUI.SetActive(false);
    }

    public void OpenEndUI()
    {
#if UNITY_ANDROID
        isJump.SetActive(false);
        isWudi.SetActive(false);
        JoyStick.SetActive(false);
#endif
        
        endUI.SetActive(true);
        Time.timeScale = 0;
        int df1 = PlayerControl.health;
        int df2 = PlayerControl.level;
        var gameTime = PlayerControl.gameTime;
        if (PlayerControl.health <= 0)
        {
            endText.text = "得分：" + 0 ;
            maxCountText.text = "最高得分："+maxCount;
            
            endImage1.SetActive(true);
            endImage2.SetActive(false);
        }
        else
        {

            float currentCount = (1000 - gameTime + df1 + df2);
            endText.text = "得分：" + currentCount;
            if (currentCount >= maxCount)
            {
                maxCount = currentCount;
                maxCountText.text = "最高得分：" + maxCount;
            }

            if (PlayerControl.useWudiNum == 0 )
            {
                currentCount = (1000 - gameTime + df1 + df2)+2500;
                endText.text = "得分：" + currentCount  + "  无敌之人！";
                if (currentCount >= maxCount)
                {
                    maxCount = currentCount;
                    maxCountText.text = "最高得分：" + maxCount;
                }
            }
            
            if (PlayerControl.beHurted == 0)
            {
                currentCount = (1000 - gameTime + df1 + df2)+2500;
                endText.text = "得分：" + currentCount  + "  无伤大佬！";
                if (currentCount >= maxCount)
                {
                    maxCount = currentCount;
                    maxCountText.text = "最高得分：" + maxCount;
                }
            }
            
            if (PlayerControl.useWudiNum == 0 && PlayerControl.beHurted == 0)
            {
                currentCount = (1000 - gameTime + df1 + df2)+5000;
                endText.text = "得分：" + currentCount  + "  神！";
                if (currentCount >= maxCount)
                {
                    maxCount = currentCount;
                    maxCountText.text = "最高得分：" + maxCount;
                }
            }
            
            endImage1.SetActive(false);
            endImage2.SetActive(true);
        }
    }

    public void StartGame()
    {

#if UNITY_ANDROID
        isJump.SetActive(true);
        isWudi.SetActive(true);
        JoyStick.SetActive(true);
#endif
        startGame = true;
        AudioManager.instance.PlayRandomMusic();
        Time.timeScale = 1;
        startUI.SetActive(false);
        AudioManager.instance.mixer.SetFloat("Ambient", AudioManager.instance.ConertSoundVolume(UIControl.instance.ambientSlider.value));
        AudioManager.instance.mixer.SetFloat("Music", AudioManager.instance.ConertSoundVolume(UIControl.instance.ambientSlider.value));

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}


