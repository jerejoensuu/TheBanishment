using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code.Player;

namespace Code.Player
{
    public class NoiseMaker : MonoBehaviour
    {
        [Range(0,100)] public float noiseMeter;
        public float noiseRange;
        [Min(1)] public float noisePerSecond = 5;
        [Min(1)] public float noiseDecay = 5;

        private bool tickDone;
        private bool saveDone;
        public bool isHiding = false;
        [SerializeField] private bool isMoving = false;
        [SerializeField] private bool enemyIsClose = false;

        private Vector3 lastPosition;
        public Vector3 noisePosition;

        private FpsMovement fpsMove;
        public GameObject enemy;

        void Start ()
        {
            fpsMove = transform.parent.GetComponent<FpsMovement>();
            tickDone = true;
            saveDone = true;
        }

        void Update()
        {
            enemyIsClose = CheckEnemyDistance();

            if (transform.position != lastPosition)
            {
                isMoving = true;
            } else 
            {
                isMoving = false;
            }

            if (tickDone)
            {
                StartCoroutine(tickMeter());
            }

            if (saveDone && !isHiding)
            {
                StartCoroutine(SavePosition());
            }

            lastPosition = transform.position;
        }
 
        private IEnumerator tickMeter()
        {
            tickDone = false;

            yield return new WaitForSeconds(0.1f);

            if (isMoving)
            {
                if (noiseMeter < 100)
                {
                    int multiplier = 1;
                    if (fpsMove.running)
                    {
                        multiplier = 5;
                    }

                    if (enemyIsClose)
                    {
                        multiplier *= 2;
                    }

                    noiseMeter += (noisePerSecond * multiplier)/10;
                } else if (noiseMeter + noisePerSecond/10 > 100) 
                {
                    noiseMeter = 100;
                }
            } else if (!isMoving) 
            {
                if (noiseMeter > 0)
                {
                    noiseMeter -= noiseDecay/10;
                } else if (noiseMeter - noiseDecay/10 < 0)
                {
                    noiseMeter = 0;
                }
            }

            noiseMeter = Mathf.Clamp(noiseMeter, 0, 100);

            tickDone = true;
        }

        private bool CheckEnemyDistance()
        {
            bool distanceCheck;

            Vector3 offset = enemy.transform.position - transform.position;
            float distanceSquared = offset.sqrMagnitude;

            if (distanceSquared < noiseRange * noiseRange)
            {
                distanceCheck = true;
            } else 
            {
                distanceCheck = false;
            }

            return distanceCheck;
        }

        private IEnumerator SavePosition()
        {
            saveDone = false;
            noisePosition = transform.position;
            yield return new WaitForSeconds(2f);
            saveDone = true;
        }
    }
}
