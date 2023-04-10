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
    public float lookingAngle;
    public int rayCount = 50;
    public GameObject Player;
    public PolygonCollider2D PolyCollider;
    private float angle;
    private float angleIncrease;
    //Time it takes for enemy to catch player in seconds
    public float timetocatch;
    public bool spottedPlayer;
    
    private float timerSeconds;
    private GameObject PlayerDetect;
    private GameObject PlayerSpotted;

    private void Start()
    {
        timerSeconds = 0;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;
        PlayerDetect = Player.transform.GetChild(0).gameObject;
        PlayerSpotted = Player.transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        SetOrigin(transform.position);
        lookingAngle = lookingAngle % 360;
        if (!spottedPlayer && timerSeconds > 0)
        {
            timerSeconds -= Time.deltaTime;
            if (timerSeconds < 0)
            {
                timerSeconds = 0;
            }
        }
        angle = lookingAngle + fov/2;
        angleIncrease = fov / rayCount;
    }

    private void LateUpdate()
    {


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

        PolyCollider.SetPath(0, path);


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
        }
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        float angle = GetAngleFromVectorFloat(aimDirection);
        lookingAngle = angle;
        SetOrigin(transform.position);

    }
    public void SetAimDirection(float aimDirection)
    {
        lookingAngle = aimDirection;
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