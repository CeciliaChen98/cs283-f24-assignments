using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BTAI;

public class BehaviorUnique : MonoBehaviour
{
    public Transform player;
    public Transform home;
    public float followRange = 10.0f;
    public float attackRange = 30.0f;
    public float wanderRadius = 20.0f;
    private bool isFollowed = false;
    private bool isAttack = false;
    private Transform targetEnemy;
    private Root m_btRoot = BT.Root();
    private Vector3 offset = new Vector3(-2, 0, 2);

    void Start()
    {
        m_btRoot.OpenBranch(
            BT.Selector().OpenBranch(
                BT.Sequence().OpenBranch(
                    BT.Condition(() => needToFollow()),
                    BT.RunCoroutine(TalkToPlayer)
                ),
                BT.Sequence().OpenBranch(
                    BT.Condition(() => IsEnemyNearBy()),
                    BT.RunCoroutine(MoveToEnemy)
                ),
                BT.Sequence().OpenBranch(
                    BT.Condition(() => isFollowPlayer()),
                    BT.RunCoroutine(MoveToPlayer)
                ),
                BT.RunCoroutine(backHome)
            )
        );
    }

    void Update()
    {
        m_btRoot.Tick();
    }

    private bool needToFollow()
    {
        if (!isFollowed)
        {
            return Vector3.Distance(transform.position, player.position) <= followRange;
        }
        else
        {
            return false;
        }
    }

    private bool IsEnemyNearBy()
    {
        if (!isFollowed)
        {
            return false;
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Loop through each enemy and check distance
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= attackRange)
            {
                targetEnemy = enemy.transform;
                isAttack = true;
                return true;
            }
        }
        return false;
    }

    private bool isFollowPlayer()
    {
        return isFollowed && (!isAttack);
    }

    IEnumerator<BTState> backHome()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        Vector3 target = home.position  +  UnityEngine.Random.insideUnitSphere * wanderRadius;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(target, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        while (agent.pathPending || agent.remainingDistance > 0.1f)
        {
            if (needToFollow())
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

        while (isFollowed&&(!isAttack))
        {
            if (IsEnemyNearBy())
            {
                yield break;
            }
            agent.SetDestination(player.position + offset);
            yield return BTState.Continue;
        }

        yield return BTState.Failure;
    }

    IEnumerator<BTState> TalkToPlayer()
    {
        Debug.Log("HERE");
        isFollowed = true;

        yield return BTState.Failure;
    }

    IEnumerator<BTState> MoveToEnemy()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        while (isAttack)
        {
            agent.SetDestination(targetEnemy.position);
            yield return BTState.Continue;
        }

        yield return BTState.Failure;
    }
}