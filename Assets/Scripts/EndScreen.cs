using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public SFXManager sfxManager;
    // Start is called before the first frame update
    void Start()
    {
        sfxManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
        sfxManager.stopMusic();
        sfxManager.playEndScreenSounds();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
