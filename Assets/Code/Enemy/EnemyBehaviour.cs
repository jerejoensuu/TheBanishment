using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Code.Player;

public class EnemyBehaviour : MonoBehaviour
{
    private int state; // 0 idle, 1 alert, 2 chasing, 3 attacking

    [Header("Movement")]
    public Vector3 lastKnownPlayerPosition;
    [Tooltip("Navigating towards this position")] public Vector3 targetDestination;
    [Tooltip("Time spent idle after reaching a destination")] public float waitTime;
    public float idleMoveSpeed;
    public float alertMoveSpeed;
    public float chaseMoveSpeed;

    [Header("Detection")]
    public float aggro = 0f;
    [Tooltip("The width of the sight cone. 0 = 180 degrees, 0.5 = 90 degrees")] public float detectionArea;
    public float idleSightRange;
    public float alertSightRange;
    public float chaseSightRange;

    [Header("Objects")]
    public Transform player;
    public NoiseMaker noiseMaker;
    public Transform[] pointsOfInterest;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = idleMoveSpeed;
    }

    private void Update()
    {
        if (aggro > 0f)
        {
            aggro -= Time.deltaTime * 5;
        }

        if (SightCheck())
        {
            aggro = 100f;
            Debug.Log("In sight");
            lastKnownPlayerPosition = player.position;
            targetDestination = lastKnownPlayerPosition;
        }
        else if (noiseMaker.noiseMeter >= 50 && state != 2)
        {
            aggro = noiseMaker.noiseMeter;
            Debug.Log("Noise");
            lastKnownPlayerPosition = player.position;
            targetDestination = lastKnownPlayerPosition;
        }
        else if (agent.remainingDistance <= 1f && state != 2)
        {
            targetDestination = pointsOfInterest[Random.Range(0, pointsOfInterest.Length)].position;
        }

        SetState(aggro);

        agent.SetDestination(targetDestination);
    }

    private void SetState(float value)
    {
        if (value > 80f) { state = 2; }
        else if (value > 40f) { state = 1; }
        else { state = 0; }

        switch (state)
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

            case 3:
            break;
        }
    }

    private bool SightCheck()
    {
        bool playerDetected = false;

        Vector3 dir = Vector3.Normalize(player.position - transform.position);
        float dot = Vector3.Dot(dir, transform.forward);

        if (dot > detectionArea) // Player is inside detection area (front of enemy)
        {
            float rayLength = idleSightRange;
            if (state == 1) { rayLength = alertSightRange; }
            else if (state == 2) { rayLength = chaseSightRange; }

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
}
