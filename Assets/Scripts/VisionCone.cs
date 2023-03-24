using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    [SerializeField] private LayerMask walllayerMask;
    [SerializeField] private LayerMask playerlayerMask;
    private Mesh mesh;
    public float fov = 90f;
    public float viewDistance = 10f;
    private Vector3 origin;
    public float startingAngle;
    public int rayCount = 50;
    public Collider2D walls;
    public GameObject Player;
    public GameObject PlayerDetect;
    public GameObject PlayerSpotted;
    public PolygonCollider2D collider;
    //Time it takes for enemy to catch player in seconds
    public float timetocatch;
    public bool spottedPlayer;

    private float timerSeconds;

    private void Start()
    {
        timerSeconds = 0;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
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
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, walllayerMask);
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


        Vector2[] path = new Vector2[vertices.Length];
        for(int i = 0; i < vertices.Length; i++)
        {
            path[i] = new Vector2(vertices[i].x, vertices[i].y);
            uv[i] = path[i].normalized;
        }

        collider.SetPath(0, path);


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject == PlayerSpotted)
            {
                spottedPlayer = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == null)
        {
            return;
        }
        if (collision.gameObject != PlayerDetect)
        {
            return;
        }
        if (timerSeconds >= timetocatch)
        {
            GameManager.Instance.GameOver();
            return;
        }

        Vector3 vectorToPlayer = Player.transform.position - transform.position;
        vectorToPlayer = new Vector3(vectorToPlayer.y, vectorToPlayer.x, vectorToPlayer.z);
        SetAimDirection(vectorToPlayer);
        timerSeconds += Time.deltaTime;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == PlayerDetect && spottedPlayer)
        {
            spottedPlayer = false;
            timerSeconds = 0;
        }
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        float angle = GetAngleFromVectorFloat(aimDirection);
        startingAngle = angle + fov / 2;
        SetOrigin(transform.position);

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
        return n;
    }
}