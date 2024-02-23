using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<AudioManager>();
            return _instance;
        }
    }

    private void Awake()
    {

        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;

        }
        else
            Destroy(this.gameObject);

    }

    private void Start()
    {
        PlayMusic("Theme");
        for (int i = 0; i < songs.Length; i++)
        {
            music.Add(new Sound(songs[i].songID, songs[i].songSound));
        }
    }
    public List <Sound> music, sfx;
    public Song[] songs;


    public AudioSource MusicSource, SfxSource, ScreenSfxSource;

    public void PlayMusic (string name)
    {
        Sound s = music.Find(x => x.name == name);
        if (s != null)
        {
            MusicSource.clip = s.clip;
            MusicSource.Play();
        }
    }

    public void PlayFromPos(string name, float pos = 0)
    {
        Sound s = music.Find(x => x.name == name);
        if (s != null)
        {
            MusicSource.clip = s.clip;
            MusicSource.time = pos;
            MusicSource.Play();
        }
        else
        {
            Debug.Log("404NotFound");
        }

    }

    public float GetMusicTime () {
        return MusicSource.time;
    }
    public float GetMusicLength()
    {
        return MusicSource.clip.length;
    }
    public void StopMusic()
    {
        MusicSource.Stop();
    }
    public void PauseMusic()
    {
        MusicSource.Pause();
    }
    

    public void PlaySfx(string name)
    {
        Sound s = sfx.Find(x => x.name == name);
        if (s != null)
        {
            SfxSource.clip = s.clip;
            SfxSource.Play();
        }
    }

    public void PlayScreenSfx(string name)
    {
        Sound s = sfx.Find(x => x.name == name);
        if (s != null)
        {
            ScreenSfxSource.clip = s.clip;
            ScreenSfxSource.Play();
        }
    }
}
