using System;
using Code.Environment;
using UnityEngine;

namespace Code.Player
{
    [RequireComponent(typeof(FpsMovement))]
    public class PlayerController : MonoBehaviour
    {
        [HideInInspector] public FpsMovement movement;
        [HideInInspector] public PlayerInteraction interaction;
        [HideInInspector] public PlayerAudio playerAudio;

        [SerializeField] private GameObject flashlight;
        private bool _flashlightOn = true;

        private void Awake()
        {
            movement = GetComponent<FpsMovement>();
            interaction = GetComponent<PlayerInteraction>();
            playerAudio = GetComponentInChildren<PlayerAudio>();
        }

        private void Update()
        {
            interaction.FireInspectionRay();
            interaction.ViewedItemHover();
        }

        public void ToggleFlashlight()
        {
            _flashlightOn = !_flashlightOn;
            flashlight.SetActive(_flashlightOn);
        }
    }
}
