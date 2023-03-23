using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;

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
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player has entered the doo2");
            animator.SetBool("Open", true);

        }
    }
}
