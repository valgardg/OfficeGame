using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishCutscene : MonoBehaviour
{
    public float cutsceneDuration;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForCutscene());
    }

    // Update is called once per frame
    IEnumerator WaitForCutscene(){
        yield return new WaitForSeconds(cutsceneDuration);
        SceneManager.LoadScene("BobOffice");
    }
}
