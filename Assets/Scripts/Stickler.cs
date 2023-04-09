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
    private float angleResetSpeed = 0.1f;
    private bool resetAngleRunning = false;
    public float fovShrink;

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
                // Shake the enemy up and down while the player is spotted
                float shakeAmount = 2f;
                transform.position += new Vector3(0, Mathf.Sin(Time.time * 40f) * shakeAmount * Time.deltaTime, 0);
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Add a 1-second delay if the player was spotted and then despotted
        if (playerDespotted)
        {
            yield return new WaitForSeconds(1f);
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
IEnumerator ResetAngle()
{
    resetAngleRunning = true;
    float currentAngle = visionCone.GetComponent<VisionCone>().lookingAngle;
    float currentFov = visionCone.GetComponent<VisionCone>().fov;
    float currentViewDistance = visionCone.GetComponent<VisionCone>().viewDistance;

    float elapsedTime = 0f;
    while ((Mathf.Abs(currentAngle - coneLookingAngle) > 0.01f || Mathf.Abs(currentFov - originalFov) > 0.01f || Mathf.Abs(currentViewDistance - originalViewDistance) > 0.01f) && !spottedPlayer)
    {
        elapsedTime += Time.deltaTime;
        float lerpFactor = elapsedTime * angleResetSpeed;

        currentAngle = Mathf.Lerp(currentAngle, coneLookingAngle, lerpFactor);
        currentFov = Mathf.Lerp(currentFov, originalFov, (lerpFactor/3));
        currentViewDistance = Mathf.Lerp(currentViewDistance, originalViewDistance, lerpFactor);

        visionCone.GetComponent<VisionCone>().lookingAngle = currentAngle;
        visionCone.GetComponent<VisionCone>().fov = currentFov;
        visionCone.GetComponent<VisionCone>().viewDistance = currentViewDistance;

        yield return null;
    }
    resetAngleRunning = false;
}



    IEnumerator SetFovOverTime(float targetFov, float duration)
    {
        float startFov = visionCone.GetComponent<VisionCone>().fov;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newFov = Mathf.Lerp(startFov, targetFov, elapsedTime / duration);
            visionCone.GetComponent<VisionCone>().fov = newFov;
            yield return null;
        }

        visionCone.GetComponent<VisionCone>().fov = targetFov;
    }

    private void toggleVisionCone(bool active)
    {
        visionCone.SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {
        bool playerCurrentlySpotted = visionCone.GetComponent<VisionCone>().spottedPlayer;
        if (!playerCurrentlySpotted)
        {
            if (!spottedPlayer && !resetAngleRunning) {
  
                StartCoroutine(ResetAngle());
            }
            
            spottedPlayer = false;

        }
        else
        {
            spottedPlayer = true;
            visionCone.GetComponent<VisionCone>().viewDistance = distance + distanceExpand;
            StartCoroutine(SetFovOverTime(fovShrink, 0.25f));
            
        }
        
    }
}
