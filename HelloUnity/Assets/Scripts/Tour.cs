using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tour : MonoBehaviour
{
    public float moveSpeed = 50.0f;
    private Transform[] POIs;
    private bool isMoving;
    private int endIndex = 0;
    private int startIndex = -1;
    private float duration;
    private int num;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {

        POIs = new Transform[] {
            GameObject.Find("POI0")?.transform,
            GameObject.Find("POI1")?.transform,
            GameObject.Find("POI2")?.transform,
            GameObject.Find("POI3")?.transform,
            GameObject.Find("POI4")?.transform,
            GameObject.Find("POI5")?.transform,
            GameObject.Find("POI6")?.transform,
            GameObject.Find("POI7")?.transform
        };

        for (int i = 0; i < POIs.Length; i++)
        {
            if (POIs[i] == null)
            {
                Debug.LogError("POI" + i + " not found!");
            }
        }
        num = POIs.Length;
        Camera.main.transform.position = POIs[endIndex].position;
        Camera.main.transform.rotation = POIs[endIndex].rotation;

        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Press 'N' to move to the next POI
        if (Input.GetKeyDown(KeyCode.N) && !isMoving)
        {
            Debug.Log("N pressed, moving to next POI");
            NextPOI();
        }
        if (isMoving)
        {
            MoveToPOI();
        }
    }

    void NextPOI()
    {
        endIndex = (endIndex + 1 ) % num;
        startIndex = (startIndex + 1) % num;

        if (POIs[endIndex] == null)
        {
            Debug.LogError("Target POI is null!");
            return;
        }

        float distance = Vector3.Distance(POIs[startIndex].position, POIs[endIndex].position);
        duration = distance / moveSpeed;
        isMoving = true;
        startTime = Time.realtimeSinceStartup;

        Debug.Log("Start Time: " + startTime + ", Duration: " + duration);
    }

    void MoveToPOI()
    {
        float u = (Time.realtimeSinceStartup - startTime) / duration;
        if (Camera.main != null)
        {
            Camera.main.transform.position = Vector3.Lerp(POIs[startIndex].position, POIs[endIndex].position, u);
            Camera.main.transform.rotation = Quaternion.Slerp(POIs[startIndex].rotation, POIs[endIndex].rotation, u);
        }
        if (u >= 1.0f)
        {
            isMoving = false;
        }
    }
}
