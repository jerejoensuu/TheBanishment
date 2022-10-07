using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public Vector3 targetPosition;
    public float waitTime;
    private NavMeshAgent agent;
    public Transform player;
    public Transform[] pointsOfInterest;
    private bool idle = true;
    private bool resting = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        targetPosition = Navigate();
    }

    private Vector3 Navigate()
    {
        if (idle && !resting) // Paths to a random point of interest
        {
            idle = false;
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
}
