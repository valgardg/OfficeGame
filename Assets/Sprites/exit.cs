using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exit : MonoBehaviour
{
    public GameObject player;
    public GameObject CannotEscapeAlert;
    public string levelLoad;
    private bool exiting;
    private bool spotted;

    private void Update()
    {

        CloseExit();

        if (GameManager.Instance.stopped)
        {
            CannotEscapeAlert.SetActive(false);
        }

        if (spotted)
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }

        if (exiting)
        {
            if (!GameManager.Instance.fadingOut)
            {
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        CannotEscapeAlert.SetActive(true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        CannotEscapeAlert.SetActive(false);
    }

    private void CloseExit()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        spotted = false;
        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i];
            VisionCone cone = enemy.GetComponentInChildren<VisionCone>();
            if (cone != null)
            {
                spotted = spotted || cone.spottedPlayer;
            }
        }
    }
}
