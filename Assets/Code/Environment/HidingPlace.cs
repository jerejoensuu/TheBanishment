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

    void Start()
    {
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
            if (!noiseMaker.hidingPlace.Contains(this.transform))
            {
                noiseMaker.hidingPlace.Add(this.transform);
            }
        }
        else
        {
            if (noiseMaker.hidingPlace.Contains(this.transform))
            {
                noiseMaker.hidingPlace.Remove(this.transform);
            }
        }
    }
}
