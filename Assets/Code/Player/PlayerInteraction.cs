﻿using System;
using Code.Environment;
using TMPro;
using UnityEngine;

namespace Code.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerInteraction : MonoBehaviour
    {
        private PlayerController _player;
        
        public float maxInteractionDistance = 3;

        private IInteractable _viewedObject;
        private TextMeshProUGUI _lookAtTextTarget;
        
        private void Awake()
        {
            _player = GetComponent<PlayerController>();
            try
            {
                _lookAtTextTarget = GameObject.FindGameObjectWithTag("LookAtText").GetComponent<TextMeshProUGUI>();
            }
            catch (Exception e)
            {
                e = e; // To get rid of warning
                Debug.LogWarning("No UI found", this);
            }
            

            Cursor.lockState = CursorLockMode.Locked;
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
                SetLookAtText("");
            }
        }

        public void ViewedItemHover()
        {
            if (_viewedObject == null) return;
            SetLookAtText(_viewedObject.LookAtText());
        }

        private void SetLookAtText(string text)
        {
            if (_lookAtTextTarget == null) return;
            _lookAtTextTarget.text = text;
        }

        public void InteractWithObject()
        {
            if (_viewedObject == null) return;
            if (!_viewedObject.IsCurrentlyInteractable()) return;
            _viewedObject.Interact();
        }
    }
}