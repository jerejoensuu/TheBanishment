using System;
using Code.Environment;
using UnityEngine;

namespace Code.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerInteraction : MonoBehaviour
    {
        private PlayerController _player;
        
        public float maxInteractionDistance = 3;

        private IInteractable _viewedObject;
        
        private void Awake()
        {
            _player = GetComponent<PlayerController>();
        }

        public void FireInspectionRay()
        {
            var camTransform = _player.movement.headCam.transform;
            Ray ray = new Ray(camTransform.position, camTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxInteractionDistance))
            {
                _viewedObject = hitInfo.transform.gameObject.GetComponentInParent<IInteractable>();
            }
            else
            {
                _viewedObject = null;
            }
        }

        public void ViewedItemHover()
        {
            // interactable.LookAtText(); //TODO: Show text in UI
        }

        public void InteractWithObject()
        {
            if (_viewedObject == null) return;
            if (!_viewedObject.IsCurrentlyInteractable()) return;
            _viewedObject.Interact();
        }
    }
}