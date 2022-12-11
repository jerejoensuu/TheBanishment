using System;
using System.Collections;
using Code.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Environment
{
    public class DoorController : MonoBehaviour, IInteractable
    {
        [SerializeField] private OpeningStyle doorOpeningStyle;
        [SerializeField] private float speed = 1;
        [SerializeField] private GameObject hinge;

        private bool _isDoorOpen;
        private bool _stateChangeInProgress;
        private PlayerController _player;
        private NavMeshObstacle navObstacle;
        public Transform enemy;
        public DoorController linkedDoor;

        private enum OpeningStyle
        {
            HingeLeft,
            HingeRight,
            AwayFromPlayer,
            TowardPlayer
        }

        [HideInInspector] public NoiseMaker noiseMaker;

        private void Start()
        {
            _player = FindObjectOfType<PlayerController>();
            noiseMaker = FindObjectOfType<NoiseMaker>();
            navObstacle = GetComponentInChildren<NavMeshObstacle>();
            if (navObstacle != null) { navObstacle.carving = _isDoorOpen; }
        }

        public void ChangeDoorState(Transform opener = null)
        {
            StartCoroutine(ChangeDoorStateCoroutine(opener));
        }

        public void ToggleDoor()
        {
            ChangeDoorState();
            if (linkedDoor != null)
            {
                linkedDoor.ChangeDoorState();
            }
        }

        public void OpenDoor(Transform opener = null)
        {
            if (_isDoorOpen) return;
            ChangeDoorState(opener);
            if (linkedDoor != null)
            {
                linkedDoor.ChangeDoorState(opener);
            }
        }

        public void CloseDoor()
        {
            if (!_isDoorOpen) return;
            ChangeDoorState();
            if (linkedDoor != null)
            {
                linkedDoor.ChangeDoorState();
            }
        }

        private IEnumerator ChangeDoorStateCoroutine(Transform opener = null)
        {
            Transform hingeTransform = hinge.transform;
            float targetAngle = _isDoorOpen ? 0 : GetDoorOpeningDirection();

            _stateChangeInProgress = true;
            while (_stateChangeInProgress && Application.isPlaying)
            {
                Vector3 hingeEulerAngles = hingeTransform.localEulerAngles;
                hingeEulerAngles = new Vector3(0,
                    Mathf.MoveTowardsAngle(hingeEulerAngles.y, targetAngle, speed * 100 * Time.deltaTime),
                    0);
                hingeTransform.localEulerAngles = hingeEulerAngles;
                yield return new WaitForSeconds(Time.deltaTime);
                _stateChangeInProgress = Math.Abs(targetAngle - hingeEulerAngles.y) > 0.5f;
            }

            hingeTransform.localEulerAngles = new Vector3(0, targetAngle, 0);

            _isDoorOpen = !_isDoorOpen;
            if (navObstacle != null) { navObstacle.carving = _isDoorOpen; }

            float GetDoorOpeningDirection()
            {
                switch (doorOpeningStyle)
                {
                    case OpeningStyle.HingeLeft:
                        return -90;
                    case OpeningStyle.HingeRight:
                        return 90;
                }

                if (!Application.isPlaying) return 90;

                Vector3 playerPosition = _player.transform.position;

                if (opener != null)
                {
                    playerPosition = opener.position;
                }

                float direction = Vector3.Dot(hingeTransform.forward, hingeTransform.position - playerPosition);
                if (doorOpeningStyle == OpeningStyle.TowardPlayer) direction *= -1;
                return Math.Sign(direction) == 1 ? 90 : -90;
            }
        }

        private void OnDrawGizmos()
        {
            Transform hingeTransform = transform.GetChild(0);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(hingeTransform.position + Vector3.down * 0.5f, Vector3.up * 5f);
        }

        public bool IsCurrentlyInteractable()
        {
            return !_stateChangeInProgress;
        }

        public string LookAtText()
        {
            return _isDoorOpen ? "Close door" : "Open door";
        }

        public void EnableHoverEffect() { }
        public void DisableHoverEffect() { }

        public void Interact()
        {
            if (enemy != null) return;

            noiseMaker.noiseMeter += 5;
            ToggleDoor();
        }

        public void OnTriggerStay(Collider col)
        {
            if (col.CompareTag("Enemy"))
            {
                enemy = col.transform;

                if (!_isDoorOpen && !_stateChangeInProgress)
                {
                OpenDoor(enemy);
                }
            }
        }

        public void OnTriggerExit(Collider col)
        {
            if (col.transform == enemy)
            {
                enemy = null;
            }
        }
    }
}