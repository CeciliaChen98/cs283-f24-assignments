using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject template;
    public float spawnRadius = 50f; 
    public int maxSpawns = 10; // number of spawned objects
    private GameObject[] screws;

    void Start()
    {
        screws = new GameObject[maxSpawns];
        for (var i = 0; i < maxSpawns; i++)
        {
            screws[i] = Instantiate(template, GetRandomPosition(), Quaternion.identity);
        }
    }

    void Update()
    {
        for (int i = 0; i < maxSpawns; i++)
        {
            if (!screws[i].activeInHierarchy && screws[i] != null)
            {
                Destroy(screws[i]);
                screws[i] = Instantiate(template, GetRandomPosition(), Quaternion.identity);
                break;
            }
        }
    }

    Vector3 GetRandomPosition()
    {
        // Create a random position within the spawn radius
        float randomX = Random.Range(-spawnRadius, spawnRadius);
        float randomZ = Random.Range(-spawnRadius, spawnRadius);

        Vector3 randomPosition = new Vector3(randomX, 0, randomZ) + transform.position;

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
