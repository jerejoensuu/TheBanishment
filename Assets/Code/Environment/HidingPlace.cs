using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code.Player;

public class HidingPlace : MonoBehaviour
{
    private BoxCollider col;
    private Bounds bounds;

    public NoiseMaker noiseMaker;
    public Transform enemyPosition;
    public Transform playerThrowPosition;
    private EnemyBehaviour enemy;

    void Start()
    {
        enemy = FindObjectOfType<EnemyBehaviour>();

        if (noiseMaker == null)
        {
            noiseMaker = FindObjectOfType<NoiseMaker>();
        }

        col = GetComponent<BoxCollider>();
        bounds = col.bounds;
    }

    void Update()
    {
        if (bounds.Contains(noiseMaker.transform.position))
        {
            if (noiseMaker.hidingPlace == null)
            {
                noiseMaker.hidingPlace = this;
            }
        }
        else
        {
            if (noiseMaker.hidingPlace == this)
            {
                noiseMaker.hidingPlace = null;
            }
        }
    }
}
