using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingIndicatorScript : MonoBehaviour
{
    private Vector3 floatVector;
    private string direction = "up";
    private float index = 0f;

    // Start is called before the first frame update
    void Start()
    {
        floatVector = new Vector3(0.0f, 0.0003f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(direction == "up"){
            transform.position += floatVector;
            index += 0.01f;
        }else{
            transform.position -= floatVector;
            index -= 0.01f;
        }

        if(index < 0){
            direction = "up";
        }
        if(index > 10){
            direction = "down";
        }
    }
}
