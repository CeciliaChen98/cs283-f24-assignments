using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoLinkController : MonoBehaviour
{
    public Transform target;     
    public Transform endEffector;
    private Transform elbow;
    private Transform root;

    // Start is called before the first frame update
    void Start()
    {
        elbow = endEffector.parent;      
        root = elbow.parent;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 R = target.position - root.position;
        float r = R.magnitude;
        Vector3 L1 = elbow.position - root.position;
        float l1 = L1.magnitude;
        Vector3 L2 = endEffector.position - elbow.position;
        float l2 = L2.magnitude;

        float cos = (l1 * l1 + l2 * l2 - r * r) / (2 * l1 * l2);
        cos = Mathf.Clamp(cos, -1f, 1f);
        float elbowDegree = 180.0f -  Mathf.Acos(cos) * Mathf.Rad2Deg;

        Debug.DrawLine(root.position, elbow.position, Color.red, 0, false); // Visualize bending axis
        Debug.DrawLine(elbow.position, endEffector.position, Color.yellow, 0, false); // Visualize bending axis
        Debug.DrawLine(endEffector.position, target.position, Color.blue, 0, false); // Visualize bending axis

        Vector3 rotationAxis = Vector3.Cross(L1,target.position-elbow.position).normalized;
        Quaternion elbowRotation = Quaternion.AngleAxis(elbowDegree, rotationAxis);
        elbow.rotation = elbowRotation * root.rotation;

        Vector3 E = target.position - endEffector.position;
        Vector3 cross = Vector3.Cross(R, E);
        float dot = Vector3.Dot(R, E);
        // Calculate the angle of rotation using atan2
        float phi = Mathf.Atan2(cross.magnitude, Vector3.Dot(R, R) + dot) * Mathf.Rad2Deg;

        rotationAxis = cross.normalized;
        Quaternion rootRotation = Quaternion.AngleAxis(phi, rotationAxis);
        root.rotation = rootRotation * root.rotation;
    }
}
