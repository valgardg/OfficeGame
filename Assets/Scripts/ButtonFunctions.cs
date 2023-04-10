using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public SFXManager sfxManager;
    // Start is called before the first frame update
    void Start()
    {
        if (sfxManager != null) sfxManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        sfxManager.stopEndScreenSounds();
        sfxManager.playMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToScene(string SceneName)
    {
        if (SceneName == "MainMenu")
        {
            Destroy(sfxManager.gameObject);
            sfxManager.stopEndScreenSounds();
            SceneManager.LoadScene(SceneName);
        }
        else
        {
            sfxManager.stopEndScreenSounds();
            sfxManager.playMusic();
            SceneManager.LoadScene(SceneName);
        }

    }
}
