using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform target;
    public float H_Dist = 10.0f;
    public float V_Dist = 12.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // tPos, tUp, tForward = Position, up, and forward vector of target
        // hDist = horizontal follow distance
        // vDist = vertical follow distance

        // Camera position is offset from the target position
        Vector3 eye = target.position - target.forward * H_Dist + target.up * V_Dist ;

        // The direction the camera should point is from the target to the camera position
        Vector3 cameraForward = target.position - eye;

        // Set the camera's position and rotation with the new values
        // This code assumes that this code runs in a script attached to the camera
        transform.position = eye;
        transform.rotation = Quaternion.LookRotation(cameraForward);
    }
}
