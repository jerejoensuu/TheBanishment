using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Code.Player;

public class EnemyBehaviour : MonoBehaviour
{
    public Vector3 targetDestination;
    public float restingTime;
    public float playerDetectionRange;
    // public float listeningRange;
    public float movementSpeed;
    public float chaseSpeed;
    private NavMeshAgent agent;
    public Transform player;
    public Vector3 playerLastKnownLocation;
    public NoiseMaker noiseMaker;
    public Transform[] pointsOfInterest;
    private bool idle = true;
    private bool resting = false;

    public GameObject DestinationIndicator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
    }

    private void Update()
    {
        targetDestination = ChangeDestination();
    }

    private Vector3 ChangeDestination()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (noiseMaker.noiseMeter >= 50 && !noiseMaker.isHiding)
        {
            playerLastKnownLocation = player.position;

            idle = false;
            resting = false;
            agent.speed = movementSpeed*1.5f;
            agent.SetDestination(playerLastKnownLocation);
        }

        if (distanceToPlayer < playerDetectionRange && !noiseMaker.isHiding) // Paths to player if nearby
        {
            idle = false;
            resting = false;
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
        }

        if (idle && !resting) // Paths to a random point of interest
        {
            idle = false;
            agent.speed = movementSpeed;
            agent.SetDestination(pointsOfInterest[Random.Range(0, pointsOfInterest.Length)].position);
        }
        else if (agent.remainingDistance < 0.5f && !resting) // Has reached a destination, waits there for a while
        {
            resting = true;
            StartCoroutine(Rest());
        }

        DestinationIndicator.transform.position = agent.destination;
        return agent.destination;
    }

    IEnumerator Rest()
    {
        yield return new WaitForSeconds(restingTime);
        idle = true;
        resting = false;
    }
}
