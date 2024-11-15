using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BTAI;

public class BehaviorMinion : MonoBehaviour
{
    public Transform player;
    public Transform home;
    public Transform center;
    public float attackRange = 20.0f;
    public float homeCheckRadius = 30.0f;
    public float wanderRadius = 80.0f;
    private Root m_btRoot = BT.Root();

    void Start()
    {
        m_btRoot.OpenBranch(
            BT.Selector().OpenBranch(
                BT.Sequence().OpenBranch(
                    BT.Condition(() => IsPlayerInRange()),
                    BT.RunCoroutine(MoveToPlayer)
                ),
                BT.Sequence().OpenBranch(
                    BT.Condition(() => IsPlayerInHomeArea()),
                    BT.RunCoroutine(MoveToRandom)
                ),
                BT.RunCoroutine(MoveToRandom)
            )
        );
    }

    void Update()
    {
        m_btRoot.Tick();
    }

    private bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    private bool IsPlayerInHomeArea()
    {
        return Vector3.Distance(home.position, player.position) <= homeCheckRadius;
    }

    IEnumerator<BTState> MoveToRandom()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        Vector3 target = center.position + UnityEngine.Random.insideUnitSphere * wanderRadius;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(target, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        while (agent.pathPending || agent.remainingDistance > 0.1f)
        {
            if (IsPlayerInRange())
            {
                yield break;
            }
            yield return BTState.Continue;
        }

        yield return BTState.Success;
    }

    IEnumerator<BTState> MoveToPlayer()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        while (IsPlayerInRange()&&(!IsPlayerInHomeArea()))
        {
            agent.SetDestination(player.position);
            yield return BTState.Continue;
        }

        yield return BTState.Failure;
    }
}