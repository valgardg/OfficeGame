using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameObject gameoverscreen;
    public bool gameover;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameover = false;
    }

    void Update()
    {
        gameoverscreen.SetActive(gameover);
    }

    public void GameOver()
    {
        gameover = true;
    }
}
