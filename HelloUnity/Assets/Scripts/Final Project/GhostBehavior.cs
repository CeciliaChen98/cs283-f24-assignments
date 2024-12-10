using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;
using BTAI;

public class GhostBehavior : MonoBehaviour
{
    public Transform player;
    public Transform home;
    public GameObject uiPanel;
    public TextMeshProUGUI output;
    public TextMeshProUGUI restart_output;
    public float attack = 30.0f;
    private float attackRange = 30.0f;
    public float homeCheckRadius = 10.0f;
    public float wanderRadius = 80.0f;
    private GameObject candle;
    private Root m_btRoot = BT.Root();
    private bool died;

    void Start()
    {
        died = false;
        candle = GameObject.Find("Character/Hand/Candle/Fire");
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }

        if (candle.activeSelf)
        {
            attackRange = attack + 10.0f;
        }
        else
        {
            attackRange = attack;
        }

        if (died&&Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }

        m_btRoot.Tick();
    }

    void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(currentScene.buildIndex); 
    }

    private bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    private bool IsPlayerInHomeArea()
    {
        return Vector3.Distance(home.position, player.position) <= homeCheckRadius;
    }

    private bool HasReachedPlayer()
    {
        return Vector3.Distance(transform.position, player.position) <= 5.0f;
    }

    IEnumerator<BTState> MoveToRandom()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Vector3 target = player.position + UnityEngine.Random.insideUnitSphere * wanderRadius;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(target, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            Debug.Log("Moving to random position: " + hit.position);
        }

        float wanderInterval = 8.0f;
        float timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;
            if (timer >= wanderInterval)
            {
                target = player.position + UnityEngine.Random.insideUnitSphere * wanderRadius;
                if (NavMesh.SamplePosition(target, out hit, wanderRadius, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                    Debug.Log("Changed destination to random position: " + hit.position);
                }
                timer = 0f; // Reset the timer
            }
            if (IsPlayerInRange()&&!IsPlayerInHomeArea())
            {
                Debug.Log("Player is in range. Stopping random movement.");
                yield break; // Exit the coroutine if the player is in range
            }

            yield return BTState.Continue;
        }
    }

    IEnumerator<BTState> MoveToPlayer()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        while (!IsPlayerInHomeArea())
        {
            agent.SetDestination(player.position);
            Debug.Log("Moving towards player: " + player.position + " attack range: "+ attackRange);

            if (HasReachedPlayer())
            {
                Debug.Log("The player got catched.");
                died = true;

                player.GetComponent<FirstPersonCamera>().enabled = false;
                GameObject light = GameObject.Find("ghost/light");
                light.transform.localPosition = new Vector3(0, 2, 1.5f);
                Vector3 startPosition = player.position + player.forward * 4.5f + player.up *3.0f;
                Vector3 targetPosition = player.position + player.forward * 2.5f + player.up * 3.0f;
                float duration = 2.0f; // Duration of the movement
                float elapsedTime = 0.0f;
                transform.LookAt(player);

                // UI setting
                uiPanel.SetActive(true);
                output.text = "You are caught!!!\nThere's nowhere left to run!!!";
                restart_output.text = "Press SPACE to restry";

                while (elapsedTime < duration)
                {
                    transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));
                    elapsedTime += Time.deltaTime;
                    yield return BTState.Continue; 
                }
                yield return BTState.Abort;
            }
            yield return BTState.Continue;
        }
        yield return BTState.Failure;
    }

}