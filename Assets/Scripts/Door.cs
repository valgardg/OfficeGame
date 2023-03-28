using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;
    public bool keyPickedUp = false;
    public GameObject keyIndicator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Player has entered the door1");
        if (other.gameObject.tag == "Player" && keyPickedUp == true)
        {
            Debug.Log("Player has entered the doo2");
            animator.SetBool("Open", true);
            Invoke("disableCollider", 0.75f);

        }
        else if (other.gameObject.tag == "Player" && keyPickedUp == false)
        {
            Debug.Log("Player has entered the door3");
            keyIndicator.SetActive(true);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            keyIndicator.SetActive(false);
        }
    }

    private void disableCollider()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
