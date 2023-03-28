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

    private Rotation rotating = Rotation.NotRotating; 
    private float originalspeed;
    private int pointIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        originalspeed = speed;
        transform.position = new Vector3(points[pointIndex].transform.position.x, points[pointIndex].transform.position.y, transform.position.z);
        pointIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponentInChildren<VisionCone>().spottedPlayer) Move();
        
    }

    void Move()
    {
        VisionCone ConeObj = Cone.GetComponent<VisionCone>();
        waypointdata PointData = points[pointIndex].GetComponent<waypointdata>();
        if (rotating != Rotation.NotRotating)
        {
            float nextAngle;

            if (rotating == Rotation.AntiClockwise)
            {
                if (ConeObj.lookingAngle >= PointData.angle)
                {
                    nextAngle = PointData.angle;
                    rotating = Rotation.NotRotating;
                }
                else nextAngle = ConeObj.lookingAngle += rotateConeSpeed*Time.deltaTime;
            }
            else
            {
                if (ConeObj.lookingAngle <= PointData.angle)
                {
                    nextAngle = PointData.angle;
                    rotating = Rotation.NotRotating;
                }
                else nextAngle = ConeObj.lookingAngle -= rotateConeSpeed * Time.deltaTime;
            }

            if (rotating == Rotation.NotRotating)
            {
                if (PointData.resetToZero)
                {
                    nextAngle = 0;
                }
                pointIndex++;
                if (pointIndex >= points.Length)
                {
                    pointIndex = 0;
                }
            }

            ConeObj.SetAimDirection(nextAngle);
        }
        else if (pointIndex <= points.Length - 1)
        {
            Vector2 newpos = Vector2.MoveTowards(transform.position, points[pointIndex].transform.position, speed*Time.deltaTime);
            transform.position = new Vector3(newpos.x, newpos.y, transform.position.z);
            if (transform.position.x == points[pointIndex].transform.position.x && transform.position.y == points[pointIndex].transform.position.y)
            {
                PointData.angle = ConvertTo360(PointData.angle);
                ConeObj.lookingAngle = ConvertTo360(ConeObj.lookingAngle);
                print(PointData.GetAngle());
                print(ConeObj.lookingAngle);
                if (PointData.angle > ConeObj.lookingAngle)
                {
                    rotating = Rotation.AntiClockwise;
                }
                else
                {
                    rotating = Rotation.Clockwise;
                }
            }
        }
        else {
            pointIndex = 0;
            Move();
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
}
