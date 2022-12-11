using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrucifixController : MonoBehaviour
{
    [SerializeField] private EnemyBehaviour enemy;
    [SerializeField] private Transform castCenter;

    [SerializeField] private float sphereRadius;
    [SerializeField] private float sphereDistance;
    [SerializeField] private float stunTime;

    private bool crucifixIsActive = false;

    private RaycastHit hit;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (castCenter == null)
        {
            castCenter = gameObject.GetComponent("CastCenter").transform;
        }
        
        //transform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!crucifixIsActive) return;
        
        if (Physics.SphereCast(castCenter.position, sphereRadius, castCenter.forward, out hit, sphereDistance))
        {
            if (hit.collider.gameObject.CompareTag("Enemy") && enemy.IsEnemyResting() == false)
            {
                Debug.Log("Hit enemy.");
                StartCoroutine(enemy.Rest(false, 3, true));
            } else if (enemy.IsEnemyResting())
            {
                Debug.Log("Enemy already resting.");
            }
        }
    }

    public void SwitchState(bool newState)
    {
        crucifixIsActive = newState;

        if (crucifixIsActive)
        {
            animator.SetTrigger("TakeOut");
        } else if (!crucifixIsActive)
        {
            animator.SetTrigger("PutAway");
        }
    }
}
