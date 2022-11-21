using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrucifixController : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform castCenter;

    [SerializeField] private float sphereRadius;
    [SerializeField] private float sphereDistance;

    private void Start()
    {
        if (castCenter == null)
        {
            castCenter = gameObject.GetComponent("CastCenter").transform;
        }
    }

    private void Update()
    {
        RaycastHit hit;

        if (Physics.SphereCast(castCenter.position, sphereRadius, castCenter.forward, out hit, sphereDistance))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                Debug.Log("Hit enemy.");
                //enemy.Stun();
            }
        }
    }
}
