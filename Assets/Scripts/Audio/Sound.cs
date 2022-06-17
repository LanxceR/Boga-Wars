using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip AudioClip;

    [Range(0f, 1f)] public float Volume = 1f;
    [Range(.1f, 3f)] public float Pitch = 1f;
    public AudioType Type;
    public bool Loop;

    [HideInInspector] public AudioSource Source;
}
