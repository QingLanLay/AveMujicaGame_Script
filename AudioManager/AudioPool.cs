using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool : MonoBehaviour 
{

    public static AudioPool instance;
    public GameObject audioPrefab;
    public int poolSize = 5;
    
    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject audio = Instantiate(audioPrefab);
            audio.transform.parent = transform;
            audio.SetActive(false);
            pool.Enqueue(audio);
        }
    }

    public GameObject GetAudio()
    {
        if (pool.Count>0)
        {
            GameObject audio = pool.Dequeue();
            audio.SetActive(true);
            return audio;
        }
        else
        {
            // 池子空了，创建播放器
            GameObject audio = Instantiate(audioPrefab);
            audio.transform.parent = transform;
            return audio;
        }
    }
    

    public IEnumerator ReturnAudio(GameObject audioPrefab)
    {
        yield return new WaitForSeconds(audioPrefab.GetComponent<AudioSource>().clip.length);
        var audioSource = audioPrefab.GetComponent<AudioSource>();
        audioSource.clip = null;
        audioPrefab.SetActive(false);
        pool.Enqueue(audioPrefab);
    }

    public void PlayGameMusic(string musicName, float volume = 1.0f)
    {
        var audioSource = GetAudio().GetComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.clip = AudioManager.instance.GetGameMusicByName(musicName);
        audioSource.Play();

        StartCoroutine(ReturnAudio(audioSource.gameObject));
    }
    
   

}
