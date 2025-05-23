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
    
    public GameObject startUI; 

    Vector3 initXiangZi;
    Vector3 initPlayer;
    Vector3 initCar;
    Vector3 initHand;
    Vector3 initFloorBase1;
    Vector3 initFloorBase2;
    Vector3 initFloorBase3;

    public static InitManager instance;
    private void Awake()
    {
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
        endUI.SetActive(true);
        Time.timeScale = 0;
        int df1 = PlayerControl.health;
        int df2 = PlayerControl.level;
        var gameTime = PlayerControl.gameTime;
        if (PlayerControl.health <= 0)
        {
            endText.text = "得分：" + 0 ;

        }
        else
        {
            endText.text = "得分：" + (1000 - gameTime + df1 + df2) ;

        }
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        startUI.SetActive(false);
    }
}

