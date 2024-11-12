using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using BTAI;
using System;
using System.Security.Cryptography;
using System.Collections.Specialized;

public class BehaviorMinion : MonoBehaviour
{
    public Transform center;  
    public Transform player;
    public float wanderRadius = 80.0f;
    private Root m_btRoot = BT.Root();

    void Start()
    {
        BTNode moveTo = BT.RunCoroutine(MoveToRandom);
        //BTNode attackPlayer = m_btRoot.RunCoroutine(MoveToPlayer);

        Sequence sequence = BT.Sequence();
        sequence.OpenBranch(moveTo);

        m_btRoot.OpenBranch(sequence);
       
    }

    void Update()
    {
        m_btRoot.Tick();

    }

    /*
    bool isPlayerNear()
    {
        NavMeshAgent agent = entity.GetComponent<NavMeshAgent>();
        return (Vector3.Distance(transform.position,player.position) < 20.f);
    }*/

    IEnumerator<BTState> MoveToRandom()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        Vector3 target;
        target = center.position + UnityEngine.Random.insideUnitSphere * wanderRadius;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(target, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        // wait for agent to reach destination
        while (agent.remainingDistance > 0.1f)
        {
            yield return BTState.Continue;
        }

        yield return BTState.Success;
    }

    IEnumerator<BTState> MoveToPlayer()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(player.position);

        // wait for agent to reach destination
        while (agent.remainingDistance > 0.1f)
        {
            yield return BTState.Continue;
        }

        yield return BTState.Success;
    }
}