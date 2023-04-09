using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameObject gameoverscreen;
    public GameObject fadeInOut;
    public float fadeInSpeed;
    public bool stopped;
    public bool gameover;
    public bool playerSpotted;

    private bool fadingIn;
    public bool fadingOut;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1;
        gameover = false;
        fadingIn = true;
        fadingOut = false;
    }

    void Update()
    {
        Color curColor = Color.black;
        if (fadeInOut)
        {
            curColor = fadeInOut.GetComponent<Image>().color;
        }
        if (fadingIn)
        {
            FadeIn(curColor);
        }
        if (fadingOut)
        {
            FadeOut(curColor);
        }
        if (gameoverscreen)
        {
            gameoverscreen.SetActive(gameover);
        }
    }

    public void Stop()
    {
        stopped = true;
        fadingOut = true;
    }

    public void GameOver()
    {
        Stop();
        Time.timeScale = 0;
        gameover = true;
    }

    public void FadeIn(Color curColor)
    {
        float curAlpha = curColor.a;
        float newAlpha;
        if (curAlpha <= 0)
        {
            fadingIn = false;
            newAlpha = 0;
        }
        else newAlpha = curAlpha - fadeInSpeed * Time.deltaTime;
        fadeInOut.GetComponent<Image>().color = new Color(curColor.r, curColor.g, curColor.b, newAlpha);
    }
    public void FadeOut(Color curColor)
    {
        float curAlpha = curColor.a;
        float newAlpha;
        if (curAlpha >= 1)
        {
            fadingOut = false;
            newAlpha = 1;
        }
        else newAlpha = curAlpha + fadeInSpeed * Time.deltaTime;
        fadeInOut.GetComponent<Image>().color = new Color(curColor.r, curColor.g, curColor.b, newAlpha);
    }
}
