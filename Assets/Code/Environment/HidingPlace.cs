using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code.Player;

public class HidingPlace : MonoBehaviour
{
    private BoxCollider col;
    private Bounds bounds;

    public NoiseMaker noiseMaker;

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
            if (!noiseMaker.isHiding)
            {
                Debug.Log("Enter");
                
            }

            noiseMaker.isHiding = true;
        } else {
            noiseMaker.isHiding = false;
        }
    }
}
