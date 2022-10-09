using System;
using System.Collections;
using UnityEngine;

namespace Code.Environment
{
    public class DoorController : MonoBehaviour, IInteractable
    {
        [SerializeField] float speed = 1;
        private bool _isDoorOpen = false;

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
            Transform hingeTransform = transform.GetChild(0);

            float targetAngle = _isDoorOpen ? 0 : 90;
            Vector3 rotation = new Vector3(0, speed, 0) * GetDoorMovementDirection();

            bool stateChangeComplete = false;
            while (!stateChangeComplete && Application.isPlaying)
            {
                hingeTransform.Rotate(rotation, Space.Self);
                yield return new WaitForSeconds(Time.deltaTime);
                stateChangeComplete = Math.Abs(targetAngle - hingeTransform.eulerAngles.y) < 0.5f;
            }

            hingeTransform.eulerAngles = new Vector3(0, targetAngle, 0);

            _isDoorOpen = !_isDoorOpen;

            float GetDoorMovementDirection()
            {
                if (_isDoorOpen) return Mathf.Sign(hingeTransform.eulerAngles.y) * -1;
                return 1; //TODO: Use player interaction side;
            }
        }

        private void OnDrawGizmos()
        {
            Transform hingeTransform = transform.GetChild(0);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(hingeTransform.position, Vector3.up * 3);
        }

        public bool IsCurrentlyInteractable()
        {
            return true;
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