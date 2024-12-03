using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject template;
    public Vector2 xRange; 
    public Vector2 zRange;
    public int y;
    public int maxSpawns = 20;
    public float minRadius = 10f;
    public float maxRadius = 100f;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(xRange.x, xRange.y),
            y,
            Random.Range(zRange.x, zRange.y)
        );

        transform.position = randomPosition;

        for (var i = 0; i < maxSpawns; i++)
        {
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            GameObject spawnedTree = Instantiate(template, GetRandomPosition(), randomRotation);
            spawnedTree.transform.localScale *= 4; // Scale the tree by 4
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 GetRandomPosition()
    {
        // Generate a random distance and angle
        float distance = Random.Range(minRadius, maxRadius);
        float angle = Random.Range(0f, 360f);

        // Convert polar coordinates to Cartesian coordinates
        float randomX = distance * Mathf.Cos(angle * Mathf.Deg2Rad);
        float randomZ = distance * Mathf.Sin(angle * Mathf.Deg2Rad);

        Vector3 randomPosition = new Vector3(randomX + xRange.y , -y-1f, randomZ + zRange.y);

        // Cast a ray downward to find the ground
        RaycastHit hit;
        if (Physics.Raycast(randomPosition + Vector3.up * 10, Vector3.down, out hit, 20f))
        {
            // Return the position on top of the terrain
            return hit.point;
        }
        return randomPosition;
    }
}
