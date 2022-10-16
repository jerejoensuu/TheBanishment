using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code.Player;

namespace Code.Player
{
    public class NoiseMaker : MonoBehaviour
    {
        [Range(0,100)]
        public float noiseMeter;
        public float noiseRange;

        [Min(1)]
        public float noisePerSecond = 5;
        [Min(1)]
        public float noiseDecay = 5;
        [HideInInspector]
        public bool tickdone;
        public bool isHiding = false;
        public bool isMoving = false;
        private Vector3 lastPosition;
        public Vector3 noisePosition;

        void Start ()
        {
            tickdone = true;
        }

        void Update()
        {
            if (transform.position != lastPosition)
            {
                isMoving = true;
                noisePosition = transform.position;
            }
            else
            {
                isMoving = false;
            }

            if (tickdone == true)
            {
                StartCoroutine(tickMeter());
            }

            lastPosition = transform.position;
        }
 
        private IEnumerator tickMeter()
        {
            tickdone = false;

            yield return new WaitForSeconds(0.1f);

            if (isMoving)
            {
                if (noiseMeter < 100)
                {
                    int multiplier = 1;
                    if (transform.parent.GetComponent<FpsMovement>().running)
                    {
                        multiplier = 5;
                    }

                    noiseMeter += (noisePerSecond * multiplier)/10;
                }
                else if (noiseMeter + noisePerSecond/10 > 100)
                {
                    noiseMeter = 100;
                }
            }
            else if (!isMoving)
            {
                if (noiseMeter > 0)
                {
                    noiseMeter -= noiseDecay/10;
                }
                else if (noiseMeter - noiseDecay/10 < 0)
                {
                    noiseMeter = 0;
                }
            }

            tickdone = true;
        }
    }
}
