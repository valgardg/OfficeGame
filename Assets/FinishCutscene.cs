using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class FinishCutscene : MonoBehaviour
{

    public SFXManager sfxManager;
    public float cutsceneDuration = 13f;
    // Start is called before the first frame update
    void Start()
    {
        VideoPlayer videoPlayer = GameObject.Find("Valgard_Anmaton").GetComponent<VideoPlayer>();
        sfxManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Valgard_Anmaton.mp4");
        print(videoPlayer.url);
        videoPlayer.Prepare();
        videoPlayer.Play();
        StartCoroutine(WaitForCutscene());
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            sfxManager.stopOfficeSounds();
            sfxManager.playMusic();
            SceneManager.LoadScene("BobOffice");
        }
    }

    // Update is called once per frame
    IEnumerator WaitForCutscene(){
        yield return new WaitForSeconds(cutsceneDuration);
        sfxManager.stopOfficeSounds();
        sfxManager.playMusic();
        SceneManager.LoadScene("BobOffice");
    }
}

/*
    }

    private void LateUpdate()
    {
        print(videoPlayer.frameCount - 1 == (ulong)videoPlayer.frame);
        print(videoPlayer.frameCount - 1 + " / " + (ulong)videoPlayer.frame);
        print(videoPlayer.isPlaying);
        print(videoPlayer.isPaused);
        if (videoPlayer.frameCount - 1 == (ulong)videoPlayer.frame && videoPlayer.isPlaying)
        {
            sfxManager.stopOfficeSounds();
            sfxManager.playMusic();
            SceneManager.LoadScene("BobOffice");
        }
    }
}
*/



