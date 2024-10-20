using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathLinear : MonoBehaviour
{
    public Transform[] positions;
    public float moveSpeed = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoLerp());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(DoLerp());
        }
    }

    IEnumerator DoLerp()
    {
        int start = 0, end = 1;
        while (true)
        {
            Vector3 startPosition = positions[start].position;
            Vector3 endPosition = positions[end].position;
            Vector3 direction = (endPosition - startPosition).normalized;

            float distance = Vector3.Distance(startPosition, endPosition);
            float duration = distance / moveSpeed;

            for (float timer = 0; timer < duration; timer += Time.deltaTime)
            {
                float u = timer / duration;
                transform.position = Vector3.Lerp(startPosition, endPosition, u);
                transform.forward = direction;
                yield return null;
            }
            start = (start + 1) % positions.Length;
            end = (end + 1) % positions.Length;
        }
    }
}
