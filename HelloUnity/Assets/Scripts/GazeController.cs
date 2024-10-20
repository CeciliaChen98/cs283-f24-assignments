using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeController : MonoBehaviour
{
    public Transform target;
    public Transform lookJoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 r = -(target.position - lookJoint.position);  
        Vector3 e = lookJoint.forward;                    

        Vector3 cross = Vector3.Cross(r, e);
        float dot = Vector3.Dot(r, e);

        // Calculate the angle of rotation using atan2
        float phi = Mathf.Atan2(cross.magnitude, Vector3.Dot(r, r) + dot) * Mathf.Rad2Deg;

        // Calculate the axis of rotation
        Vector3 axis = cross.normalized;
        Quaternion computedRot = Quaternion.AngleAxis(phi, axis);

        lookJoint.parent.rotation = computedRot * lookJoint.parent.rotation;

        // Debugging
        Debug.DrawLine(lookJoint.position, target.position, Color.red);
    }
}
