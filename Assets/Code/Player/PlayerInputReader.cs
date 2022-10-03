using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Player
{
    public class PlayerInputReader : MonoBehaviour
    {
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
            // _inputs.Player.Run.performed += ReadRun;
            // _inputs.Player.Run.canceled += ReadRun;
        }

        private void ReadMovement(InputAction.CallbackContext context)
        {
            _player.movement.ReceiveInput(context.ReadValue<Vector2>());
        }

        private void ReadRun(InputAction.CallbackContext context)
        {
            _player.movement.running = context.performed;
        }
    }
}