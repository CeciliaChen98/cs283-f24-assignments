using UnityEngine;
using UnityEngine.AI;

public class Wander : MonoBehaviour
{
    public float wanderRadius = 100f;
    public float destinationReachedThreshold = 1f;

    private NavMeshAgent NPC;

    void Start()
    {
        NPC = GetComponent<NavMeshAgent>();
        SetRandomDestination();
    }

    void Update()
    {
        if (NPC.remainingDistance <= destinationReachedThreshold && !NPC.pathPending)
        {
            SetRandomDestination();
        }
    }

    // Sets a new random point as the agent's destination
    private void SetRandomDestination()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * wanderRadius;
        NavMeshHit hit;

        // Check if the random point is on the NavMesh
        if (NavMesh.SamplePosition(randomPoint, out hit, wanderRadius, NavMesh.AllAreas))
        {
            NPC.SetDestination(hit.position);
        }
    }
}
