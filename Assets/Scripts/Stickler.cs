using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickler : MonoBehaviour
{
    public float peekInterval;
    public float peekDistance;
    public float peekSpeed;
    public float peekDuration;

    private Vector3 initialPosition;
    public GameObject visionCone;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(PeekRoutine());
    }

    IEnumerator PeekRoutine()
    {
        while (true)
        {
            // Wait for the peekInterval
            yield return new WaitForSeconds(peekInterval);

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
        
    }
}
