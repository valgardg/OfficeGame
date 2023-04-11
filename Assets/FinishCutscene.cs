using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishCutscene : MonoBehaviour
{

    public SFXManager sfxManager;
    public float cutsceneDuration;
    // Start is called before the first frame update
    void Start()
    {
        sfxManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
        StartCoroutine(WaitForCutscene());
    }

    // Update is called once per frame
    IEnumerator WaitForCutscene(){
        yield return new WaitForSeconds(cutsceneDuration);
        sfxManager.stopOfficeSounds();
        sfxManager.playMusic();
        SceneManager.LoadScene("BobOffice");
    }
}
