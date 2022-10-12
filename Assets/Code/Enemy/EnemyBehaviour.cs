using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    private int state; // 0 idle, 1 alert, 2 chasing, 3 attacking


    [Header("Movement")]
    [Tooltip("Navigating towards this position")] public Vector3 targetDestination;
    [Tooltip("Time spent idle after reaching a destination")] public float waitTime;
    public float idleMoveSpeed;
    public float chaseMoveSpeed;

    [Header("Detection")]
    [Tooltip("The width of the sight cone. 0 = 180 degrees, 0.5 = 90 degrees")] public float detectionArea;
    public float idleSightRange;
    public float alertSightRange;
    public float chaseSightRange;

    [Header("Objects")]
    public Transform player;
    public Transform[] pointsOfInterest;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = idleMoveSpeed;
    }

    private void Update()
    {
        if (SightCheck())
        {
            SetState(2);
        }
        else
        {
            SetState(0);
        }
        //else if (SoundCheck())
        //{
        //    SetState(1);
        //}

        //targetDestination = ChangeDestination();
    }

    private void SetState(int value)
    {
        state = value;
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
                    Debug.Log(playerDetected);
                    Debug.DrawRay(transform.position, (player.position - transform.position) * hit.distance, Color.yellow);
                }
            }

        }
        return playerDetected;
    }

    /*
    private Vector3 ChangeDestination()
    {


        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < idleSightRange) // Paths to player if nearby
        {
            idle = false;
            resting = false;
            agent.speed = chaseMoveSpeed;
            agent.SetDestination(player.position);
        }

        if (idle && !resting) // Paths to a random point of interest
        {
            idle = false;
            agent.speed = idleMoveSpeed;
            agent.SetDestination(pointsOfInterest[Random.Range(0, pointsOfInterest.Length)].position);
        }
        else if (agent.remainingDistance < 0.5f && !resting) // Has reached a destination, waits there for a while
        {
            resting = true;
            StartCoroutine(Rest());
        }

        return agent.destination;
    }

    IEnumerator Rest()
    {
        yield return new WaitForSeconds(waitTime);
        idle = true;
        resting = false;
    }
    */
}
