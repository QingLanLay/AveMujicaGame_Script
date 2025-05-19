using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public List<GameObject> obstacles;

    public void ResetState()
    {
        StartCoroutine(ForeachOba());
    }

    private IEnumerator ForeachOba()
    {
        foreach (var oba in obstacles)
        {
            int randomInt = UnityEngine.Random.Range(0, 100);
            if (randomInt > 49)
            {
                oba.SetActive(true);
                oba.GetComponent<ObaGroup>().ResetState();
                yield return null;
            }
            else
            {
                oba.SetActive(false);
                oba.GetComponent<ObaGroup>().CloseAll();
            }
        }
    }
}