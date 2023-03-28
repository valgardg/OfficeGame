using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour
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

    private Rotation rotating = Rotation.NotRotating; 
    private bool fixingRotation;
    private float originalrotation;
    private int pointIndex = 0;
    private VisionCone ConeObj;
    private waypointdata PointData;
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        ConeObj = Cone.GetComponent<VisionCone>();
        distance = ConeObj.viewDistance;
        originalrotation = points[pointIndex].GetComponent<waypointdata>().angle;
        transform.position = new Vector3(points[pointIndex].transform.position.x, points[pointIndex].transform.position.y, transform.position.z);
        pointIndex++;
        PointData = points[pointIndex].GetComponent<waypointdata>();
        fixingRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool playerspotted = (GetComponentInChildren<VisionCone>().spottedPlayer);
        if (!playerspotted && fixingRotation)
        {
            ConeObj.viewDistance = distance;
            ConeObj.lookingAngle = originalrotation;
            fixingRotation = false;
            rotating = Rotation.NotRotating;
        }
        else if (!playerspotted && rotating != Rotation.NotRotating)
        {
            Rotate(originalrotation);
            if (rotating == Rotation.NotRotating)
            {
                if (PointData.resetToZero)
                {
                    ConeObj.SetAimDirection(0f);
                    originalrotation = 0f;
                }
                pointIndex++;
                if (pointIndex >= points.Length)
                {
                    pointIndex = 0;
                }

                PointData = points[pointIndex].GetComponent<waypointdata>();
            }
        }
        else if (!playerspotted) Move();
        else
        {
            ConeObj.viewDistance = distance + distanceExpand;
            rotating = Rotation.NotRotating;
            fixingRotation = true;
        }
        
    }

    void Move()
    {
        
        Vector2 newpos = Vector2.MoveTowards(transform.position, points[pointIndex].transform.position, speed*Time.deltaTime);
        transform.position = new Vector3(newpos.x, newpos.y, transform.position.z);
        if (transform.position.x == points[pointIndex].transform.position.x && transform.position.y == points[pointIndex].transform.position.y)
        {
            Rotate(PointData.angle);
            originalrotation = PointData.angle;
        }
        
    }
    public float ConvertTo360(float angle)
    {
        if (angle < 0)
        {
            return 180 + (-angle);
        }
        return angle;
    }

    public void Rotate(float angle)
    {
        if (rotating != Rotation.NotRotating)
        {
            float nextAngle;

            if (rotating == Rotation.AntiClockwise)
            {
                if (ConeObj.lookingAngle >= angle)
                {
                    nextAngle = angle;
                    rotating = Rotation.NotRotating;

                }
                else nextAngle = ConeObj.lookingAngle += rotateConeSpeed * Time.deltaTime;
            }
            else
            {
                if (ConeObj.lookingAngle <= angle)
                {
                    nextAngle = angle;
                    rotating = Rotation.NotRotating;
                }
                else nextAngle = ConeObj.lookingAngle -= rotateConeSpeed * Time.deltaTime;
            }
            ConeObj.SetAimDirection(nextAngle);
        }
        else
        {
            if (ConvertTo360(angle) > ConvertTo360(ConeObj.lookingAngle))
            {
                rotating = Rotation.AntiClockwise;
            }
            else
            {
                rotating = Rotation.Clockwise;
            }
        }
    }
}
