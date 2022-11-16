﻿using System;
using Code.Level;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Player
{
    public class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private PlayerStamina stamina;

        private PlayerController _player;

        private PlayerInputActions _inputs;

        private void Awake()
        {
            _player = GetComponent<PlayerController>();
            _inputs = new PlayerInputActions();
        }

        private void OnEnable()
        {
            _inputs.Enable();

            _inputs.Player.Move.performed += ReadMovement;
            _inputs.Player.Move.canceled += ReadMovement;
            _inputs.Player.Run.performed += ReadRun;
            _inputs.Player.Run.canceled += ReadRun;
            _inputs.Player.Fire.performed += Interact;
            _inputs.Player.Fire.canceled += Interact;
            _inputs.Player.Sneak.performed += ReadSneak;
            _inputs.Player.Sneak.canceled += ReadSneak;

            _inputs.Player.Flashlight.performed += ToggleFlashlight;
            _inputs.Player.Flashlight.canceled += ToggleFlashlight;

            _inputs.Player.Menu.performed += Escape;
            _inputs.Player.Menu.canceled += Escape;
        }

        private void ReadMovement(InputAction.CallbackContext context)
        {
            _player.movement.ReceiveInput(context.ReadValue<Vector2>());
        }

        private void ReadRun(InputAction.CallbackContext context)
        {
            if (!stamina.outOfStamina)
            {
                _player.movement.running = context.performed;
            }
        }

        private void Interact(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _player.interaction.InteractWithObject();
            }
        }

        private void ReadSneak(InputAction.CallbackContext context)
        {
            _player.movement.sneaking = context.performed;
        }

        private void ToggleFlashlight(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _player.ToggleFlashlight();
            }
        }

        private void Escape(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                FindObjectOfType<LevelManager>().ResetLevel();
            }
        }
    }
}