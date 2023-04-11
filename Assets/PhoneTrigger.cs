using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneTrigger : MonoBehaviour
{
    public GameObject guard;
    public GameObject keyIndicator;
    private GuardPath guardPath;
    public bool ringing;

    public AudioSource phoneRing;

    // Start is called before the first frame update
    void Start()
    {
        guardPath = guard.GetComponent<GuardPath>();
        ringing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(ringing && !phoneRing.isPlaying)
        {
            phoneRing.Play();
        }
        if(!ringing)
        {
            phoneRing.Stop();
        }
    }
    
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player") && !ringing){
            keyIndicator.SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            if(Input.GetKey(KeyCode.E)){
                guardPath.guarding = false;
                Debug.Log(guardPath.guarding);
                keyIndicator.SetActive(false);
                ringing = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            keyIndicator.SetActive(false);
        }
    }
}
