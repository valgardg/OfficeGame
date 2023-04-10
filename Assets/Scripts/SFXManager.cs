using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private static SFXManager instance = null;
    public AudioSource officeSounds;
    public AudioSource musicSource;
    public AudioSource endScreenSound;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void playMusic()
    {
        Debug.Log("Playing music");
        musicSource.Play();
    }

    public void stopOfficeSounds()
    {
        Debug.Log("Stopping office sounds");
        officeSounds.Stop();
    }

    public void playOfficeSounds()
    {
        officeSounds.Play();
    }

    public void playEndScreenSounds()
    {
        endScreenSound.Play();
    }
}

