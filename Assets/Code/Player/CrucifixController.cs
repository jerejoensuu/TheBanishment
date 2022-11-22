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

    private void Start()
    {
        if (castCenter == null)
        {
            castCenter = gameObject.GetComponent("CastCenter").transform;
        }
        
        transform.gameObject.SetActive(false);
    }

    private void Update()
    {
        RaycastHit hit;

        if (Physics.SphereCast(castCenter.position, sphereRadius, castCenter.forward, out hit, sphereDistance))
        {
            if (hit.collider.gameObject.tag == "Enemy" && enemy.IsEnemyResting() == false)
            {
                Debug.Log("Hit enemy.");
                StartCoroutine(enemy.Rest(false, 3));
            } else if (enemy.IsEnemyResting() == true)
            {
                Debug.Log("Enemy already resting.");
            }
        }
    }
}
