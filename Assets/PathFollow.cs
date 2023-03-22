using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
    public GameObject[] points;
    public float speed = 2f;

    private int pointIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(points[pointIndex].transform.position.x, points[pointIndex].transform.position.y, transform.position.z);
        pointIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (pointIndex <= points.Length - 1)
        {
            Vector2 newpos = Vector2.MoveTowards(transform.position, points[pointIndex].transform.position, speed*Time.deltaTime);
            transform.position = new Vector3(newpos.x, newpos.y, transform.position.z);
            if (transform.position.x == points[pointIndex].transform.position.x && transform.position.y == points[pointIndex].transform.position.y)
            {
                pointIndex++;
            }
        }
        else {
            pointIndex = 0;
            Move();
        }
    }
}
