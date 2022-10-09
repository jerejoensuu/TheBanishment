using System;
using System.Collections;
using Code.Player;
using UnityEngine;

namespace Code.Environment
{
    public class DoorController : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject hinge;
        [SerializeField] private float speed = 1;
        private bool _isDoorOpen;
        private bool _stateChangeInProgress;

        public void ToggleDoor()
        {
            StartCoroutine(ChangeDoorState());
        }

        public void OpenDoor()
        {
            if (_isDoorOpen) return;
            StartCoroutine(ChangeDoorState());
        }

        public void CloseDoor()
        {
            if (!_isDoorOpen) return;
            StartCoroutine(ChangeDoorState());
        }

        private IEnumerator ChangeDoorState()
        {
            Transform hingeTransform = hinge.transform;
            float targetAngle = _isDoorOpen ? 0 : GetDoorOpeningDirection();

            _stateChangeInProgress = true;
            while (_stateChangeInProgress && Application.isPlaying)
            {
                Vector3 hingeEulerAngles = hingeTransform.eulerAngles;
                hingeEulerAngles = new Vector3(0,
                    Mathf.MoveTowardsAngle(hingeEulerAngles.y, targetAngle, speed * 100 * Time.deltaTime),
                    0);
                hingeTransform.eulerAngles = hingeEulerAngles;
                yield return new WaitForSeconds(Time.deltaTime);
                _stateChangeInProgress = Math.Abs(targetAngle - hingeEulerAngles.y) > 0.5f;
            }

            hingeTransform.eulerAngles = new Vector3(0, targetAngle, 0);

            _isDoorOpen = !_isDoorOpen;

            float GetDoorOpeningDirection()
            {
                if (!Application.isPlaying) return 90;

                Vector3 playerPosition = FindObjectOfType<PlayerController>().transform.position;
                float direction = Vector3.Dot(hingeTransform.forward, hingeTransform.position - playerPosition);
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

        public void Interact()
        {
            ToggleDoor();
        }
    }
}