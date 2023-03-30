using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exit : MonoBehaviour
{
    public GameObject player;
    public string levelLoad;
    private bool exiting;

    private void Update()
    {
        if (exiting)
        {
            print("exiting");
            if (!GameManager.Instance.fadingOut)
            {
                SceneManager.LoadScene(levelLoad);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            GameManager.Instance.Stop();
            exiting = true;
        }
    }
}
