using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{

    public GameObject Player;
    public float xOffset;
    public float yOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Player.transform.position.x + xOffset, Player.transform.position.y + yOffset, transform.position.z);

    }
}
