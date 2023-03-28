using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    public GameObject Door;

        // Variables for wobble effect
    private float originalY;
    private float originalScale;
    public float wobbleDistance;
    public float wobbleSpeed;
    public float resizeAmount;

    // Start is called before the first frame update
    void Start()
    {
        originalY = transform.position.y;
        originalScale = transform.localScale.x;
        
    }

    // Update is called once per frame
    void Update()
    {
        // Make key move up and down slightly
        float newY = originalY + Mathf.Sin(Time.time * wobbleSpeed) * wobbleDistance;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // // Resize key slightly
        float newScale = originalScale + Mathf.Sin(Time.time * wobbleSpeed) * resizeAmount;
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Door.GetComponent<Door>().keyPickedUp = true;
            this.gameObject.SetActive(false);
        }
    }
}
