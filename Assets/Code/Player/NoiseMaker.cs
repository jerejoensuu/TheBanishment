using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code.Player;
using UnityEngine.UI;

namespace Code.Player
{
    public class NoiseMaker : MonoBehaviour
    {
        [Range(0, 100)] public float noiseMeter;
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
        public float alertValue;

        private FpsMovement fpsMove;
        public GameObject enemy;

        [SerializeField] private Slider noiseBar;
        [SerializeField] private Image barFill;

        private void Start()
        {
            fpsMove = transform.parent.GetComponent<FpsMovement>();
            tickDone = true;
            saveDone = true;
        }

        private void Update()
        {
            enemyIsClose = CheckEnemyDistance();

            isMoving = transform.position != lastPosition;

            if (tickDone)
            {
                StartCoroutine(TickMeter());
            }

            if (saveDone && !isHiding)
            {
                StartCoroutine(SavePosition());
            }

            AdjustBar();
            lastPosition = transform.position;
        }

        private IEnumerator TickMeter()
        {
            tickDone = false;

            yield return new WaitForSeconds(0.1f);

            if (isMoving)
            {
                if (noiseMeter < 100)
                {
                    float multiplier = 1;
                    float soundAmount = 30;
                    
                    if (fpsMove.running)
                    {
                        multiplier = 5;
                        soundAmount = 60;
                    }

                    if (fpsMove.sneaking)
                    {
                        multiplier = 0.3f;
                        soundAmount = 15;
                    }

                    if (enemyIsClose)
                    {
                        multiplier *= 2;
                    }

                    // noiseMeter += (noisePerSecond * multiplier) / 10f;
                    noiseMeter = soundAmount;
                }
            }
            else if (!isMoving)
            {
                if (noiseMeter > 0)
                {
                    noiseMeter -= noiseDecay / 10;
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
            }
            else
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

        private void AdjustBar()
        {
            noiseBar.value = noiseMeter;

            if (noiseMeter < alertValue - (alertValue / 5))
            {
                barFill.color = Color.green;
            }
            else if (noiseMeter > (alertValue / 5) && noiseMeter < alertValue)
            {
                barFill.color = Color.yellow;
            }
            else if (noiseMeter > alertValue)
            {
                barFill.color = Color.red;
            }
        }
    }
}