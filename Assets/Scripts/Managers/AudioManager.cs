using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum AudioType
{
    MUSIC,
    SFX
}
public enum MusicTrack
{
    TRACK_ONE,
    TRACK_TWO,
    TRACK_THREE
}
public class AudioManager : MonoBehaviour
{
    // Singleton instance
    private static AudioManager instance;

    [SerializeField] private Sound[] sounds;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Add audiosource for every sfx
        foreach (Sound sound in sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.AudioClip;

            sound.Source.volume = sound.Volume;
            sound.Source.pitch = sound.Pitch;
            sound.Source.loop = sound.Loop;
        }

        DontDestroyOnLoad(gameObject);
        ChangeMusic("Track 1");
    }

    // Get singleton instance
    public static AudioManager GetInstance()
    {
        return instance;
    }

    public void PlayAudio(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.Name == name);
        sound.Source.Play();
    }
    public void StopAudio(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.Name == name);
        sound.Source.Stop();
    }

    public void PlayPlayerShoot()
    {
        PlayAudio("Player Shoot");
    }
    public void PlayEnemyShoot()
    {
        PlayAudio("Enemy Shoot");
    }
    public void PlayPlayerHit()
    {
        PlayAudio("Player Hit");
    }
    public void PlayEnemyHit()
    {
        PlayAudio("Enemy Hit");
    }
    public void PlayRoomClear()
    {
        PlayAudio("Room Clear");
    }
    public void PlayButtonClickSfx()
    {
        PlayAudio("Button Click");
    }
    public void PlayFanfareSfx()
    {
        PlayAudio("Fanfare");
    }

    public void ChangeMusic(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.Type == AudioType.MUSIC)
            {
                if (sound.Name != name)
                    sound.Source.Stop();
                else if (!sound.Source.isPlaying)
                    sound.Source.Play();
            }
        }
    }
    public void ChangeMusic(MusicTrack track)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].Type == AudioType.MUSIC)
            {
                if (sounds[i] != sounds[(int)track])
                    sounds[i].Source.Stop();
                else if (!sounds[i].Source.isPlaying)
                    sounds[i].Source.Play();
            }
        }
    }
}
