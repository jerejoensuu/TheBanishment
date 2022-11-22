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
        public List<Transform> hidingPlace = new List<Transform>();
        [SerializeField] private bool isMoving = false;
        [SerializeField] private bool enemyIsClose = false;
        [SerializeField] private bool enemyIsFar = false;

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
            if (Input.GetKeyDown("n"))
            {
                Debug.Log("Delete this");
                noiseMeter = 60;
            }

            CheckEnemyDistance();

            isMoving = transform.position != lastPosition;

            if (tickDone)
            {
                StartCoroutine(TickMeter());
            }

            if (saveDone && hidingPlace.Count == 0)
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
                    
                    if (fpsMove.running)
                    {
                        multiplier = 5;
                    }

                    if (fpsMove.sneaking)
                    {
                        multiplier = 0.3f;
                    }

                    if (enemyIsClose)
                    {
                        multiplier *= 2;
                    }

                    if (enemyIsFar)
                    {
                        multiplier *= 0.5f;
                    }

                    noiseMeter += (noisePerSecond * multiplier) / 10f;
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

        private void CheckEnemyDistance()
        {
            Vector3 offset = enemy.transform.position - transform.position;
            float distanceSquared = offset.sqrMagnitude;

            if (distanceSquared < noiseRange)
            {
                enemyIsClose = true;
            }
            else
            {
                enemyIsClose = false;
            }

            if (distanceSquared > noiseRange*4)
            {
                enemyIsFar = true;
            }
            else
            {
                enemyIsFar = false;
            }
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