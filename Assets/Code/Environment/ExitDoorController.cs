using System;
using System.Collections;
using Code.Level;
using Code.Player;
using UnityEngine;

namespace Code.Environment
{
    public class ExitDoorController : MonoBehaviour, IInteractable
    {
        [SerializeField] private OpeningStyle doorOpeningStyle;
        [SerializeField] private float speed = 0.35f;
        [SerializeField] private float openDoorAngle = 25;
        [SerializeField] private GameObject hinge;

        private bool _isDoorOpen;
        private bool _stateChangeInProgress;
        private PlayerController _player;
        private LevelManager _levelManager;

        private enum OpeningStyle
        {
            HingeLeft,
            HingeRight,
            AwayFromPlayer,
            TowardPlayer
        }

        private void Start()
        {
            _levelManager = FindObjectOfType<LevelManager>();
            _player = FindObjectOfType<PlayerController>();
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

            float GetDoorOpeningDirection()
            {
                switch (doorOpeningStyle)
                {
                    case OpeningStyle.HingeLeft:
                        return -openDoorAngle;
                    case OpeningStyle.HingeRight:
                        return openDoorAngle;
                }

                if (!Application.isPlaying) return openDoorAngle;

                Vector3 playerPosition = _player.transform.position;

                float direction = Vector3.Dot(hingeTransform.forward, hingeTransform.position - playerPosition);
                if (doorOpeningStyle == OpeningStyle.TowardPlayer) direction *= -1;
                return Math.Sign(direction) == 1 ? openDoorAngle : -openDoorAngle;
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
            return !_stateChangeInProgress && IsExitOpen() && !_isDoorOpen;
        }

        public string LookAtText()
        {
            return IsCurrentlyInteractable() ? "Exit level" : "\"I can't leave yet\"";
        }

        public void Interact()
        {
            OpenDoor();
            _levelManager.EndLevel();
        }

        private bool IsExitOpen()
        {
            return _levelManager.WinningConditionsMet;
        }
    }
}