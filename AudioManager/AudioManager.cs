using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour 
{
    public static AudioManager instance;
    public List<AudioClip> musicLists;

    public AudioSource gameMusic;
    public AudioSource gameMusic2;
    public AudioSource ambientMusic;


    public List<AudioClip> ambientClips;
    
    public AudioMixer mixer;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {   
        // 检测当前音乐是否播放完毕
        if (!ambientMusic.isPlaying && ambientClips.Count > 0 && InitManager.instance.startGame)
        {
            PlayRandomMusic();
        }
    }

    public void PlayGameMusic(string musicName,float volume =1f)
    {
        gameMusic.clip = musicLists.Find(music => music.name == musicName);
        gameMusic.Play();
    }
    public void PlayGameMusic2(string musicName , float volume =1f)
    {
        gameMusic2.clip = musicLists.Find(music => music.name == musicName);
        gameMusic2.volume = volume;
        if (!gameMusic2.isPlaying)
        {
            gameMusic2.Play();
        }
    }

    public void PlayRandomMusic()
    {
        // 随机选择一首音乐
        int randomIndex = Random.Range(0, ambientClips.Count);
        ambientMusic.clip = ambientClips[randomIndex];
        ambientMusic.Play();
    }

    public AudioClip GetGameMusicByName(string musicName)
    {
        gameMusic.clip = musicLists.Find(music => music.name == musicName);
        return gameMusic.clip;
    }

    public float ConertSoundVolume(float amount)
    {
        return (amount * 100 - 80);
    }
}

