using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringFollowCamera : MonoBehaviour
{
    public Transform target;             
    public float H_Dist = 10.0f; 
    public float V_Dist = 5.0f;     
    public float springConstant = 10.0f;   
    public float dampConstant = 5.0f;

    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Camera position is offset from the target position
        Vector3 idealEye = target.position - target.forward * H_Dist + target.up * V_Dist;

        // The direction the camera should point is from the target to the camera position
        Vector3 cameraForward = target.position - transform.position;

        // Compute the acceleration of the spring, and then integrate
        Vector3 displacement = transform.position - idealEye;
        Vector3 springAccel = (-springConstant * displacement) - (dampConstant * velocity);

        // Update the camera's velocity based on the spring acceleration
        velocity += springAccel * Time.deltaTime;

        // Set the camera's position and rotation with the new values
        // This code assumes that this code runs in a script attached to the camera
        transform.position += velocity * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(cameraForward);

    }
}
