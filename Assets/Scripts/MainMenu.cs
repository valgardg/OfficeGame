using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject controlsMenuUI;
    public GameObject Title;
    public GameObject Picture;


    // public void Awake()
    // {
    //     PauseMenu.GameIsPaused = true;
    // }

    public void PlayGame()
    // When we build, the menu should be at index 0 in our build settings. The game will be at index 1.
    {
        Debug.Log("Play Game");
        // PauseMenu.GameIsPaused = false;
        SceneManager.LoadScene(0); // Loads level 1
    }

    // Closes the game, doesn't really work in the website version. But does work when you build the game as an application!
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void ControlsMenu()
    {
        Title.SetActive(false);
        mainMenuUI.SetActive(false);
        controlsMenuUI.SetActive(true);
        Picture.SetActive(false);
    }

    public void BackToMainMenu()
    {
        GameObject controlsMenu = GameObject.Find("ControlsMenu");
        controlsMenuUI.SetActive(false);
        Title.SetActive(true);
        mainMenuUI.SetActive(true);
        Picture.SetActive(true);
    }
}
