using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;
    public float fov = 90f;
    public float viewDistance = 10f;
    private Vector3 origin;
    public float startingAngle;
    public int rayCount = 50;
    public Collider2D walls;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        fov = 90f;
        viewDistance = 10f;
        origin = Vector3.zero;
    }

    private void Update()
    {
        SetOrigin(transform.position);
        startingAngle = startingAngle % 360;
    }

    private void LateUpdate()
    {
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = transform.InverseTransformPoint(origin);

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
            if (raycastHit2D.collider == null)
            {
                // No hit
                vertex = transform.InverseTransformPoint(origin + GetVectorFromAngle(angle) * viewDistance);
            }
            else
            {
                // Hit object
                vertex = transform.InverseTransformPoint(raycastHit2D.point);
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetFoV(float fov)
    {
        this.fov = fov;
    }

    public void SetViewDistance(float viewDistance)
    {
        this.viewDistance = viewDistance;
    }

    public Vector3 GetVectorFromAngle(float angle)
    {
        angle *= Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
    }
    public float GetAngleFromVectorFloat(Vector3 vector)
    {
        vector = vector.normalized;
        float n = Mathf.Atan2(vector.x, vector.y) * Mathf.Rad2Deg;
        return n % 360;
    }
}