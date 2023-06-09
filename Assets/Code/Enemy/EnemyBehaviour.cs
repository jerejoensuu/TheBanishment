using System.Collections;
using Code.Level;
using UnityEngine;
using UnityEngine.AI;
using Code.Player;

public class EnemyBehaviour : MonoBehaviour
{
    // 0 oblivious, 1 alert, 2 chasing
    private int state
    {
        get => behaviourState;
        set
        {
            behaviourState = value;
            if (musicManager != null)
            {
                musicManager.musicState = value switch
                {
                    0 => MusicManager.MusicState.Ambient,
                    1 => MusicManager.MusicState.Ambient,
                    2 => MusicManager.MusicState.Chase,
                    _ => MusicManager.MusicState.Ambient
                };
            }
        }
    }

    private int behaviourState;

    private bool resting;

    [Header("Movement")]
    public Vector3 lastKnownPlayerPosition;

    [Tooltip("Navigating towards this position")]
    public Vector3 targetDestination;

    [Tooltip("Time spent idle after reaching a destination")]
    public float restTime;

    public float idleMoveSpeed;
    public float alertMoveSpeed;
    public float chaseMoveSpeed;

    [Header("Detection")]
    
    public float sightRange;

    private bool playerDetected = false;
    private float detectionArea = 0.05f; //The width of the sight area. 0 = 180 degrees, 0.5 = 90 degrees

    [Tooltip("Multiply sight range by n when flashlight is on")]
    public float flashlightSightMultiplier;

    [Tooltip("Remains in alert state for n seconds after investigating noise")]
    public float alertDuration;

    [Tooltip("Remains in chase state for n seconds after losing sight of player")]
    public float chaseDuration;

    private float alertTimeElapsed;
    private float chaseTimeElapsed;
    private bool investigating = false;

    [Header("Attack")]
    public float attackDamage = 45f;

    [Tooltip("Enemy takes a break for n seconds after attacking the player")]
    public float attackCooldown;

    private bool throwAttack = false;
    public float throwDuration;

    [Header("Objects")]
    public Transform player;
    private MusicManager musicManager;
    public Transform[] pointsOfInterest;
    private NavMeshAgent agent;
    private NoiseMaker noiseMaker;
    private PlayerController playerController;
    private PlayerHealth playerHealth;
    public GameObject DestinationIndicator;
    public Animator animator;

    [Header("Hard mode")]
    public float hardIdleMoveSpeed = 5.5f;
    public float hardAttackDamage = 70f;
    public float hardFlashlightSightMultiplier = 3.5f;


    private void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
        noiseMaker = player.GetComponentInChildren<NoiseMaker>();
        playerController = player.GetComponent<PlayerController>();
        playerHealth = player.GetComponent<PlayerHealth>();
        agent = GetComponent<NavMeshAgent>();

        alertTimeElapsed = alertDuration;
        chaseTimeElapsed = chaseDuration;

        SetPath(pointsOfInterest[Random.Range(0, pointsOfInterest.Length)].position);

        if (PlayerPrefs.GetInt("levelProgress") > 1)
        {
            idleMoveSpeed = hardIdleMoveSpeed;
            attackDamage = hardAttackDamage;
            flashlightSightMultiplier = hardFlashlightSightMultiplier;
        }
    }

    private void Update()
    {
        playerDetected = SightCheck();

        if (playerDetected ||
            Vector3.Distance(transform.position, player.position) < 1.5f) // Chase when player is in sight or touching
        {
            state = 2;
            chaseTimeElapsed = 0f;

            if (noiseMaker.hidingPlace == null)
            {
                lastKnownPlayerPosition = player.position;
                SetPath(lastKnownPlayerPosition);
            }
        }
        else if
            (noiseMaker.noiseMeter >= noiseMaker.alertValue ||
             state == 2) // Investigate last player position when hearing noise of when losing track of player after chasing
        {
            investigating = true;
            state = 1;
            alertTimeElapsed = 0f;
            lastKnownPlayerPosition = noiseMaker.noisePosition;
            SetPath(lastKnownPlayerPosition);
        }

        if (noiseMaker.hidingPlace != null && state == 2)
        {
            throwAttack = true;
            SetPath(noiseMaker.hidingPlace.enemyPosition.position);
        }

        if (noiseMaker.hidingPlace == null)
        {
            throwAttack = false;
        }

        if (!throwAttack)
        {
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
        }

        if (agent.hasPath && ((agent.remainingDistance <= 3f && !throwAttack) ||
                              (agent.remainingDistance <= 0.75f && throwAttack)))
        {
            TargetReached();
        }

        SetSpeed();
    }

    private void SetSpeed()
    {
        animator.SetBool("isWalking", !resting);

        if (resting)
        {
            agent.speed = 0f;
        }
        else
        {
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
            }
        }
        animator.SetFloat("moveSpeed", agent.speed / 2);
    }

    private void TargetReached()
    {
        agent.ResetPath();

        switch (state)
        {
            case 0: // Path to random point of interest if no input from player
                SetPath(pointsOfInterest[Random.Range(0, pointsOfInterest.Length)].position);
                StartCoroutine(Rest(true));
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

                if (nearestPoint == targetDestination)
                {
                    Debug.Log("!");
                    nearestPoint = pointsOfInterest[Random.Range(0, pointsOfInterest.Length)].position;
                }

                SetPath(nearestPoint);
                StartCoroutine(Rest(true));
                break;

            case 2: // Attack player, then rest for a while
                if (!resting)
                {
                    if (throwAttack)
                    {
                        Debug.Log("Enemy attacks (closet)");
                        StartCoroutine(ThrowPlayer(noiseMaker.hidingPlace.playerThrowPosition.position));
                    }
                    else
                    {
                        Debug.Log("Enemy attacks");
                    }

                    SetPath(lastKnownPlayerPosition);

                    StartCoroutine(Rest(false, attackCooldown));
                    
                    animator.SetTrigger("attack");
                    playerHealth.TakeDamage(attackDamage);
                }

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
        bool seesPlayer = false;

        float multiplier = 1f;
        if (playerController.FlashlightOn())
        {
            multiplier = flashlightSightMultiplier;
            detectionArea = 0.05f;
        }
        else
        {
            detectionArea = 0.25f;
        }

        Vector3 dir = Vector3.Normalize(player.position - transform.position);
        float dot = Vector3.Dot(dir, transform.forward);

        if (dot > detectionArea) // Player is inside detection area (front of enemy)
        {
            float rayLength = sightRange;
            if (state == 1)
            {
                rayLength = sightRange * 2;
            }
            else if (state == 2)
            {
                rayLength = sightRange * 10;
            }

            rayLength *= multiplier;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, (player.position - transform.position), out hit, rayLength,
                    LayerMask.GetMask("Default"),
                    QueryTriggerInteraction.Ignore)) // Raycast towards player to see if anything's blocking vision
            {
                if (hit.transform.gameObject.tag == "Player") // Ray hits player
                {
                    seesPlayer = true;
                }
            }
        }

        return seesPlayer;
    }

    public IEnumerator ThrowPlayer(Vector3 targetPosition)
    {
        targetPosition.y = player.position.y;
        Vector3 startingPosition = player.position;
        float timeElapsed = 0f;

        while (timeElapsed < throwDuration)
        {
            player.position = Vector3.Lerp(startingPosition, targetPosition, timeElapsed / throwDuration);
            Physics.SyncTransforms();
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        player.position = targetPosition;
        Physics.SyncTransforms();
    }

    public IEnumerator Rest(bool interruptable, float changeTime = 0f, bool stunned = false)
    {
        if (stunned) animator.SetBool("isStunned", true);
        int startingState = state;

        float time = restTime;
        if (changeTime > 0f)
        {
            time = changeTime;
        }

        resting = true;

        for (float timeElapsed = 0f; timeElapsed < time; timeElapsed += Time.deltaTime)
        {
            if (!interruptable)
            {
                transform.LookAt(player.transform);
            }

            if (interruptable && state > startingState)
            {
                resting = false;
                investigating = false;
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }

        investigating = false;
        resting = false;
        animator.SetBool("isStunned", false);
    }

    public bool IsEnemyResting()
    {
        return resting;
    }
}