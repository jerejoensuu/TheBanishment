using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Code.Player;

public class EnemyBehaviour : MonoBehaviour
{
    public int state; // 0 oblivious, 1 alert, 2 chasing
    private bool resting;

    [Header("Movement")]
    public Vector3 lastKnownPlayerPosition;
    [Tooltip("Navigating towards this position")] public Vector3 targetDestination;
    [Tooltip("Time spent idle after reaching a destination")] public float restTime;
    public float idleMoveSpeed;
    public float alertMoveSpeed;
    public float chaseMoveSpeed;

    [Header("Detection")]
    private bool playerDetected = false;
    [Tooltip("The width of the sight area. 0 = 180 degrees, 0.5 = 90 degrees")] public float detectionArea;
    public float sightRange;
    [Tooltip("Remains in alert state for n seconds after hearing noise")] public float alertDuration;
    [Tooltip("Remains in chase state for n seconds after losing sight of player")] public float chaseDuration;
    public float alertTimeElapsed;
    public float chaseTimeElapsed;
    public bool investigating = false;
    
    [Tooltip("Enemy takes a break for n seconds after attacking the player")] public float attackCooldown;
    private float attackTimeElapsed;

    [Header("Objects")]
    public Transform player;
    public Transform[] pointsOfInterest;
    private NavMeshAgent agent;
    private NoiseMaker noiseMaker;

    public GameObject DestinationIndicator;

    private void Start()
    { 
        noiseMaker = player.GetComponentInChildren<NoiseMaker>();
        agent = GetComponent<NavMeshAgent>();

        alertTimeElapsed = alertDuration;
        chaseTimeElapsed = chaseDuration;
        attackTimeElapsed = attackCooldown;
       
        SetPath(pointsOfInterest[0].position);
    }

    private void Update()
    {
        if (attackTimeElapsed < attackCooldown) // Do nothing for a while after attacking
        {
            attackTimeElapsed += Time.deltaTime;
        }
        else
        {
            if (SightCheck() || Vector3.Distance(transform.position, player.position) < 1.5f) // Chase when player is in sight or touching
            {
                state = 2;
                chaseTimeElapsed = 0f;
                lastKnownPlayerPosition = player.position;
                SetPath(player.position);
            }
            else if (noiseMaker.noiseMeter >= 40f || state == 2) // Investigate last player position when hearing noise of when losing track of player after chasing
            {
                investigating = true;
                state = 1;
                alertTimeElapsed = 0f;
                lastKnownPlayerPosition = noiseMaker.noisePosition;
                SetPath(lastKnownPlayerPosition);
            }

            if (chaseTimeElapsed < chaseDuration) // Remain in chase state for a short duration when losing sight
            {
                chaseTimeElapsed += Time.deltaTime;
                lastKnownPlayerPosition = player.position;
                SetPath(lastKnownPlayerPosition);
                state = 2;
            }
            else if (alertTimeElapsed < alertDuration)
            {
                if (!investigating) // Start counting down from alert state after investigating noise position
                {
                    alertTimeElapsed += Time.deltaTime;
                }
                state = 1;
            }
            else
            {
                state = 0;
            }

            if (agent.hasPath && agent.remainingDistance <= 2f)
            {
                TargetReached();
            }

            SetSpeed();
        }
    }

    private void SetSpeed()
    {
        if (resting)
        {
            agent.speed = 0f;
        }
        else switch (state)
        {
            case 0:
            agent.speed = idleMoveSpeed;
            break;

            case 1:
            agent.speed = alertMoveSpeed;
            break;

            case 2:
            agent.speed = chaseMoveSpeed;
            break;
        }
    }

    private void TargetReached()
    {
        agent.ResetPath();

        switch (state)
            {
                case 0: // Path to random point of interest if no input from player
                    SetPath(pointsOfInterest[Random.Range(0, pointsOfInterest.Length)].position);
                    StartCoroutine(Rest());
                    break;

                case 1: // Path to nearest point of interest after investigating a noise
                    float shortestDistance = Vector3.Distance(pointsOfInterest[0].position, transform.position);
                    Vector3 nearestPoint = pointsOfInterest[0].position;

                    for (int i = 1; i < pointsOfInterest.Length; i++)
                    {
                        if (Vector3.Distance(pointsOfInterest[i].position, transform.position) < shortestDistance)
                        {
                            nearestPoint = pointsOfInterest[i].position;
                        }
                    }

                    SetPath(nearestPoint);
                    StartCoroutine(Rest());
                    break;

                case 2: // Attack player, then rest for a while
                    if (Vector3.Distance(transform.position, player.position) < 3f)
                    {
                        Debug.Log("Enemy attacks");
                        player.GetComponent<PlayerHealth>().TakeDamage(45f);
                    }

                    attackTimeElapsed = 0f;
                    SetPath(lastKnownPlayerPosition);
                    StartCoroutine(Rest(attackCooldown));
                    break;
            }
    }

    private void SetPath(Vector3 target)
    {
        targetDestination = target;
        agent.SetDestination(targetDestination);
    }

    private bool SightCheck()
    {
        bool inSight = false;
        Vector3 dir = Vector3.Normalize(player.position - transform.position);
        float dot = Vector3.Dot(dir, transform.forward);

        if (dot > detectionArea) // Player is inside detection area (front of enemy)
        {
            float rayLength = sightRange;
            if (state == 1) { rayLength = sightRange * 2; }
            else if (state == 2) { rayLength = sightRange * 10; }

            RaycastHit hit;
            if (Physics.Raycast(transform.position, (player.position - transform.position), out hit, rayLength, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore)) // Raycast towards player to see if anything's blocking vision
            {
                if (hit.transform.gameObject.tag == "Player") // Ray hits player
                {
                    inSight = true;
                }
            }

        }

        playerDetected = inSight;
        return inSight;
    }

    IEnumerator Rest(float changeTime = 0f)
    {
        state = 0;
        float time = restTime;
        if (changeTime > 0f)
        {
            time = changeTime;
        }

        resting = true;

        yield return new WaitForSeconds(time);
        if (investigating)
        {
            investigating = false;
        }
        
        resting = false;
    }
}
