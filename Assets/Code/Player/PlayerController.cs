using System;
using System.Collections;
using System.Collections.Generic;
using Code.Environment;
using UnityEngine;
using TMPro;

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

        [SerializeField] private GameObject crucifix;
        private CrucifixController crucifixController;
        [SerializeField] private float crucifixCooldown;
        [SerializeField] private float crucifixTime;
        private bool offCooldown = true;
        [SerializeField] private int crucifixAmount;
        [SerializeField] private TextMeshProUGUI crucifixText; 

        private void Awake()
        {
            movement = GetComponent<FpsMovement>();
            interaction = GetComponent<PlayerInteraction>();
            playerAudio = GetComponentInChildren<PlayerAudio>();
            crucifixController = crucifix.GetComponent<CrucifixController>();

            UpdateCrucifixText();
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

        public bool FlashlightOn()
        {
            return _flashlightOn;
        }

        public void EnableCrucifix()
        {
            StartCoroutine(CrucifixActive());
        }

        private IEnumerator CrucifixActive()
        {
            crucifixAmount--;
            UpdateCrucifixText();
            offCooldown = false;
            crucifixController.SwitchState(true);

            yield return new WaitForSeconds(crucifixTime);
            
            crucifixController.SwitchState(false);
            StartCoroutine(CrucifixCooldown());
        }

        private IEnumerator CrucifixCooldown()
        {
            yield return new WaitForSeconds(crucifixCooldown);
            offCooldown = true;
        }

        public void AddCrucifix()
        {
            if (crucifixAmount < 3)
            {
                crucifixAmount++;
            }
            UpdateCrucifixText();
        }

        public bool CrucifixCheck()
        {
            bool isAvailable = false;

            if (offCooldown && crucifixAmount>0)
            {
                isAvailable = true;
            }

            return (isAvailable);
        }

        private void UpdateCrucifixText()
        {
            crucifixText.text = crucifixAmount.ToString();
        }
    }
}
