using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public List<GameObject> obstacles;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // 重置状态
    public void ResetState()
    {
        StartCoroutine(ForeachOba());
    }

    // 随机开启对应的障碍小组
    private IEnumerator ForeachOba()
    {
        foreach (var oba in obstacles)
        {
            int randomInt = UnityEngine.Random.Range(0, 100);
            if (randomInt > 100- PlayerControl.level*10 )
            {
                oba.SetActive(true);
                if (oba.name != "anon")
                {
                    oba.GetComponent<ObaGroup>().ResetState();
                }
                yield return null;
            }
            else
            {
                oba.SetActive(false);
                if (oba.name != "anon")
                {
                    oba.GetComponent<ObaGroup>().CloseAll();

                }
            }

            if (oba.name == "anon")
            {
                int randomInt2 = UnityEngine.Random.Range(0, 100);
                if (PlayerControl.level < 5)
                {
                    oba.GetComponent<AnonControl>().ResetState();
                }
                else
                {
                    if (randomInt2 >= 60)
                    {
                        oba.GetComponent<AnonControl>().ResetState();
                    }
                    else
                    {
                        oba.GetComponent<AnonControl>().Close();
                    }
                }
            }
        }
    }
}
