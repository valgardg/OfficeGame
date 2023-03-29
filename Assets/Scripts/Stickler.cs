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

    private Vector3 initialPosition;
    public GameObject visionCone;
    public Animator animator;
    public GameObject warning;

    public float coneLookingAngle;
    // Start is called before the first frame update
    void Start()
    {
        coneLookingAngle = visionCone.GetComponent<VisionCone>().lookingAngle;
        initialPosition = transform.position;
        warning.SetActive(false);
        StartCoroutine(PeekRoutine());

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
            yield return new WaitForSeconds(peekDuration);
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
            visionCone.GetComponent<VisionCone>().lookingAngle = coneLookingAngle;
        }
        
    }
}
