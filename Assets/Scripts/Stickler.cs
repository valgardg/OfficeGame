using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickler : MonoBehaviour
{
    public float peekInterval;
    public float warningTime;
    public float peekDistance;
    public float peekSpeed;
    public float peekDuration;
    private float distance;
    public float distanceExpand;
    public bool spottedPlayer;
    private float originalFov;
    private float originalViewDistance;

    private Vector3 initialPosition;
    public GameObject visionCone;
    public Animator animator;
    public GameObject warning;

    public float coneLookingAngle;
    // Start is called before the first frame update
    void Start()
    {
        coneLookingAngle = visionCone.GetComponent<VisionCone>().lookingAngle;
        distance = visionCone.GetComponent<VisionCone>().viewDistance;
        initialPosition = transform.position;
        warning.SetActive(false);
        StartCoroutine(PeekRoutine());
        spottedPlayer = false;
        originalFov = visionCone.GetComponent<VisionCone>().fov;
        originalViewDistance = visionCone.GetComponent<VisionCone>().viewDistance;

    }

IEnumerator PeekRoutine()
{
    while (true)
    {
        
        // Wait for the peekInterval
        yield return new WaitForSeconds(peekInterval);
        warning.SetActive(true);
        yield return new WaitForSeconds(warningTime);
        warning.SetActive(false);

        

        // Move up
        float targetY = initialPosition.y + peekDistance;
        while (transform.position.y < targetY)
        {
            float step = peekSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y + step, transform.position.z);
            yield return null;
        }


        // Wait for the peekDuration
        toggleVisionCone(true);
        animator.SetBool("Looking", true);
        
        // Custom WaitForSeconds loop
        float elapsedTime = 0f;
        bool playerDespotted = false;
        while (elapsedTime < peekDuration || spottedPlayer)
        {
            if (spottedPlayer)
            {
                playerDespotted = true;
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Add a 1-second delay if the player was spotted and then despotted
        if (playerDespotted)
        {
            yield return new WaitForSeconds(2f);
        }

        animator.SetBool("Looking", false);
        toggleVisionCone(false);

        // Move back down
        while (transform.position.y > initialPosition.y)
        {
            float step = peekSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y - step, transform.position.z);
            yield return null;
        }
    }
}





    private void toggleVisionCone(bool active)
    {
        visionCone.SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {
        if (!visionCone.GetComponent<VisionCone>().spottedPlayer)
        {
            spottedPlayer = false;
            visionCone.GetComponent<VisionCone>().lookingAngle = coneLookingAngle;
            visionCone.GetComponent<VisionCone>().fov = originalFov; 
            visionCone.GetComponent<VisionCone>().viewDistance = originalViewDistance;
        }
        else
        {
            spottedPlayer = true;
            visionCone.GetComponent<VisionCone>().viewDistance = distance + distanceExpand;
            visionCone.GetComponent<VisionCone>().fov = 20f;
            
        }
        
    }
}
