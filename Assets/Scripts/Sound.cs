using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public Sound(string name, AudioClip clip)
    {
        this.name = name;
        this.clip = clip;
    }
}
