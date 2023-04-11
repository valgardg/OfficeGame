using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPath : MonoBehaviour
{
    enum Rotation
    {
        NotRotating,
        Clockwise,
        AntiClockwise
    }
    public GameObject[] points;
    public GameObject Cone;
    public float speed = 2f;
    public float rotateConeSpeed = 2f;
    public float distanceExpand;
    public float fovShrink;
    public bool loop = true;
    public Animator animator;

    public int inspectindex;
    public bool guarding = true;
    public float inspectDuration = 2.0f;
    private bool isInspecting;

    private Rotation rotating = Rotation.NotRotating; 
    private bool fixingRotation;
    private float originalrotation;
    private int pointIndex = 0;
    private VisionCone ConeObj;
    private waypointdata PointData;
    private float distance;
    private float originalFov;
    private float originalViewDistance;
    private bool playerspotted;
    // Start is called before the first frame update
    public GameObject phoneTriggerObject;
    public AudioSource phoneClickAudio;
    private PhoneTrigger phoneTrigger;
    
    void Start()
    {
        ConeObj = Cone.GetComponent<VisionCone>();
        distance = ConeObj.viewDistance;
        originalrotation = points[pointIndex].GetComponent<waypointdata>().angle;
        transform.position = new Vector3(points[pointIndex].transform.position.x, points[pointIndex].transform.position.y, transform.position.z);
        pointIndex++;
        PointData = points[pointIndex].GetComponent<waypointdata>();
        fixingRotation = false;
        originalFov = ConeObj.fov;
        phoneTrigger = phoneTriggerObject.GetComponent<PhoneTrigger>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(isInspecting){return;}
        bool playerspotted = ConeObj.spottedPlayer;
        if (!playerspotted && fixingRotation)
        {
            // ConeObj.viewDistance = distance;
            StartCoroutine(ResetAngle());
            ConeObj.lookingAngle = originalrotation;
            fixingRotation = false;
            rotating = Rotation.NotRotating;
        }
        else if (!playerspotted && rotating != Rotation.NotRotating)
        {
            Rotate(originalrotation);
            if (rotating == Rotation.NotRotating)
            {
                pointIndex++;
                if(pointIndex == inspectindex){
                    Debug.Log("shoud stop now and inspect for a second");
                    StartCoroutine(PauseCoroutine());
                }
                if (pointIndex >= points.Length)
                {
                    pointIndex = 0;
                    guarding = true;
                }
                PointData = points[pointIndex].GetComponent<waypointdata>();
            }
        }
        else if (!playerspotted) {
            if(guarding){return;}
            Move();
            Animate();
            // StartCoroutine(ResetAngle());
        }
        else
        {
            ConeObj.viewDistance = distance + distanceExpand;
            StartCoroutine(SetFovOverTime(fovShrink, 0.25f));
            rotating = Rotation.NotRotating;
            fixingRotation = true;
            float shakeAmount = 2f;
            transform.position += new Vector3(0, Mathf.Sin(Time.time * 40f) * shakeAmount * Time.deltaTime, 0);
        }
        
    }

    IEnumerator PauseCoroutine() {
        // Set the object to be paused
        isInspecting = true;

        // Wait for the specified duration
        yield return new WaitForSeconds(inspectDuration);

        // Set the object to be unpaused
        isInspecting = false;
        phoneTrigger.ringing = false;
        phoneClickAudio.Play();
    }

    void Move()
    {
        
        Vector2 newpos = Vector2.MoveTowards(transform.position, points[pointIndex].transform.position, speed*Time.deltaTime);
        transform.position = new Vector3(newpos.x, newpos.y, transform.position.z);
        if (transform.position.x == points[pointIndex].transform.position.x && transform.position.y == points[pointIndex].transform.position.y)
        {
            originalrotation = PointData.angle;
            Rotate(originalrotation);
            /*//Placeholder since Rotate function is buggy
            pointIndex++;
            if (pointIndex >= points.Length)
            {
                if (loop) pointIndex = 0;
                else pointIndex = points.Length-1;
            }
            PointData = points[pointIndex].GetComponent<waypointdata>();
            ///////////////////////////////////////////*/
        }

    }

    void Animate()
    {
        Vector2 direction = (points[pointIndex].transform.position - transform.position).normalized;
        float moveHorizontal = direction.x;
        float moveVertical = direction.y;

        if (direction.sqrMagnitude > 0.1)
        {
            animator.SetFloat("x", moveHorizontal);
            animator.SetFloat("y", moveVertical);
        }

        animator.SetFloat("speed", direction.magnitude);
    }

    public float ConvertTo360(float angle)
    {
        if (angle < 0)
        {
            return 180 + (-angle);
        }
        return angle % 360;
    }

    IEnumerator ResetAngle()
    {
        // resetAngleRunning = true;
        float currentAngle = ConeObj.lookingAngle;
        float currentFov = ConeObj.fov;
        float currentViewDistance = ConeObj.viewDistance;
        float angleResetSpeed = 0.1f;

        float elapsedTime = 0f;
        while ((Mathf.Abs(currentFov - originalFov) > 0.01f && !playerspotted) || (Mathf.Abs(currentViewDistance - distance) > 0.01f && playerspotted))
        {
            elapsedTime += Time.deltaTime;
            float lerpFactor = elapsedTime * angleResetSpeed;


            currentFov = Mathf.Lerp(currentFov, originalFov, (lerpFactor/3));
            currentViewDistance = Mathf.Lerp(currentViewDistance, distance, (lerpFactor));

            ConeObj.fov = currentFov;
            ConeObj.viewDistance = currentViewDistance;

            yield return null;
        }
        // resetAngleRunning = false;
    }



    IEnumerator SetFovOverTime(float targetFov, float duration)
    {
        float startFov = ConeObj.fov;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newFov = Mathf.Lerp(startFov, targetFov, elapsedTime / duration);
            ConeObj.fov = newFov;
            yield return null;
        }

        ConeObj.fov = targetFov;
    }

    public void Rotate(float angle)
    {
        float DAngle = Mathf.Abs(ConvertTo360(ConeObj.lookingAngle) - ConvertTo360(angle));
        if (rotating != Rotation.NotRotating)
        {
            float nextAngle;
            if (rotating == Rotation.AntiClockwise)
            {
                nextAngle = ConeObj.lookingAngle += rotateConeSpeed * Time.deltaTime;
                DAngle -= rotateConeSpeed * Time.deltaTime;
                if (DAngle <= 0)
                {
                    nextAngle = angle;
                    rotating = Rotation.NotRotating;

                }
            }
            else
            {
                nextAngle = ConeObj.lookingAngle -= rotateConeSpeed * Time.deltaTime;
                DAngle -= rotateConeSpeed * Time.deltaTime;
                if (DAngle <= 0)
                {
                    nextAngle = angle;
                    rotating = Rotation.NotRotating;
                }
            }
            ConeObj.SetAimDirection(nextAngle);
        }
        else
        {
            if (PointData.turningClockwise)
            {
                rotating = Rotation.Clockwise;
            }
            else
            {
                rotating = Rotation.AntiClockwise;
            }
        }
    }
}
