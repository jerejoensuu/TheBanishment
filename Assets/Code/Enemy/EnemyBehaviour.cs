using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Code.Player;

public class EnemyBehaviour : MonoBehaviour
{
    public int state; // 0 oblivious, 1 alert, 2 chasing
    private bool resting = false;

    [Header("Movement")]
    public Vector3 lastKnownPlayerPosition;
    [Tooltip("Navigating towards this position")] public Vector3 targetDestination;
    [Tooltip("Time spent idle after reaching a destination")] public float restTime;
    public float idleMoveSpeed;
    public float alertMoveSpeed;
    public float chaseMoveSpeed;

    [Header("Detection")]
    [Tooltip("The width of the sight area. 0 = 180 degrees, 0.5 = 90 degrees")] public float detectionArea;
    public float sightRange;
    [Tooltip("Remains in alert state for n seconds after hearing noise")] public float alertDuration;
    [Tooltip("Remains in chase state for n seconds after losing sight of player")] public float chaseDuration;
    private float alertTimeElapsed;
    private float chaseTimeElapsed;

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
       
        SetPath(pointsOfInterest[0].position);
    }

    private void Update()
    {
        if (chaseTimeElapsed < chaseDuration)
        {
            chaseTimeElapsed += Time.deltaTime;
            state = 2;
        }
        else if (alertTimeElapsed < alertDuration)
        {
            alertTimeElapsed += Time.deltaTime;
            state = 1;
        }
        else
        {
            state = 0;
        }

        if (SightCheck())
        {
            state = 2;
            chaseTimeElapsed = 0f;
            SetPath(player.position);
        }
        else if (noiseMaker.noiseMeter >= 40f)
        {
            state = 1;
            alertTimeElapsed = 0f;
            noiseMaker.noiseMeter = 30f;
            lastKnownPlayerPosition = noiseMaker.noisePosition;
            SetPath(lastKnownPlayerPosition);
        }

        if (agent.hasPath && agent.remainingDistance <= 2f)
        {
            TargetReached();
        }

        SetSpeed();
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

                case 2:
                Debug.Log("Enemy attacks");
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
        bool playerDetected = false;
        Vector3 dir = Vector3.Normalize(player.position - transform.position);
        float dot = Vector3.Dot(dir, transform.forward);

        if (dot > detectionArea) // Player is inside detection area (front of enemy)
        {
            float rayLength = sightRange;
            if (state == 1) { rayLength = sightRange * 2; }
            else if (state == 2) { rayLength = sightRange * 10; }

            RaycastHit hit;
            if (Physics.Raycast(transform.position, (player.position - transform.position), out hit, rayLength)) // Raycast towards player to see if anything's blocking vision
            {
                if (hit.transform.gameObject.tag == "Player") // Ray hits player
                {
                    playerDetected = true;
                }
            }

        }
        return playerDetected;
    }

    IEnumerator Rest()
    {
        resting = true;
        yield return new WaitForSeconds(restTime);

        if (state > 0)
        {
            resting = false;
            yield break;
        }

        resting = false;
    }
}
