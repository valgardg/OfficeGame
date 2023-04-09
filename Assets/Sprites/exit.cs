using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exit : MonoBehaviour
{
    public GameObject player;
    public string levelLoad;
    private bool exiting;
    private bool spotted;


    private void Update()
    {

        CloseExit();

        print(spotted);

        if (spotted)
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }

        if (exiting)
        {
            if (!GameManager.Instance.fadingOut)
            {
                print(levelLoad);
                SceneManager.LoadScene(levelLoad);
            }
        }

        if (!GetComponent<BoxCollider2D>().isTrigger && !spotted)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player && !spotted)
        {
            GameManager.Instance.Stop();
            exiting = true;
        }
    }

    private void CloseExit()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        spotted = false;
        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i];
            print(enemies[i].name);
            spotted = spotted || enemy.GetComponentInChildren<VisionCone>().spottedPlayer;
        }
    }
}
