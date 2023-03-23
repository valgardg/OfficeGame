using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HideObjectScript : MonoBehaviour
{
    public GameObject hidingObject;
    public TextMeshProUGUI hideText;
    public GameObject hidingIndicator;
    public GameObject keyIndicator;
    public SpriteRenderer spriteRender;
    public Sprite defaultSprite;
    public Sprite highlightedSprite;
    private GameObject player;
    private bool canHide;
    private bool hiding;
    private Vector3 previousPlayerPosition;
    public Vector3 hidePosition;

    // Start is called before the first frame update
    void Start()
    {
        canHide = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(hiding){
            DisplayStopHiding();
        }

        if(Input.GetKeyDown(KeyCode.E) && hiding){
                Debug.Log("unhifde player!");
                UnhidePlayer();
        }else if(Input.GetKeyDown(KeyCode.E) && canHide){
                Debug.Log("hide player!");
                HidePlayer();
        }

    }

    void HidePlayer(){
        // set game manager state to player hidden
        // set the players previous locatoin into variable
        previousPlayerPosition = player.transform.position;
        // move game player to hide area
        player.transform.position = new Vector3(6.0f, 2.0f, 0.0f);
        // set hiding to true
        hiding = true;
        Debug.Log(hiding);
        player.SetActive(false);
        spriteRender.sprite = defaultSprite;
        hidingIndicator.SetActive(true);
        keyIndicator.SetActive(false);
        if(this.transform.parent.gameObject.CompareTag("HideableTable")){
            this.transform.parent.gameObject.transform.Rotate(0f,0f,-5f);
        }
    }

    void UnhidePlayer(){
        player.transform.position = previousPlayerPosition;
        hiding = false;
        hideText.text = "";
        player.SetActive(true);
        hidingIndicator.SetActive(false);
    }

    void DisplayStopHiding(){
        //hideText.text = "Press E to stop hiding!";
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player") && this.transform.parent.gameObject.CompareTag("HideableTable")){
            this.transform.parent.gameObject.transform.Rotate(0f,0f,5f);
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if(hiding){
            return;
        }

        if(other.gameObject.CompareTag("Player")){
            player = other.gameObject;
            //hideText.text = "Press E to hide!";
            keyIndicator.SetActive(true);
            canHide = true;
            spriteRender.sprite = highlightedSprite;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(hiding){
            return;
        }

        if(other.gameObject.CompareTag("Player")){
            //hideText.text = "";
            keyIndicator.SetActive(false);
            canHide = false;
            spriteRender.sprite = defaultSprite;
            if(this.transform.parent.gameObject.CompareTag("HideableTable")){
                this.transform.parent.gameObject.transform.Rotate(0f,0f,-5f);
            }
        }
    }
}
