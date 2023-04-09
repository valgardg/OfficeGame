using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottedIndicator : MonoBehaviour
{
    public GameObject visionCone;
    public bool spotted;
    public Animator animator;
    public SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        spotted = false;
    }

    // Update is called once per frame
    void Update()
    {
        spotted = visionCone.GetComponent<VisionCone>().spottedPlayer;
        if (spotted)
        {
            animator.SetBool("Spotted", true);
            spriteRenderer.enabled = true;
            if (GameManager.Instance.gameover)
            {
                spriteRenderer.enabled = false;
            }
        }
        else
        {
            animator.SetBool("Spotted", false);
            spriteRenderer.enabled = false;
        }
    }
}
