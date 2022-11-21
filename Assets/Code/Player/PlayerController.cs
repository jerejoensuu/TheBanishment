using System;
using System.Collections;
using System.Collections.Generic;
using Code.Environment;
using UnityEngine;

namespace Code.Player
{
    [RequireComponent(typeof(FpsMovement))]
    public class PlayerController : MonoBehaviour
    {
        [HideInInspector] public FpsMovement movement;
        [HideInInspector] public PlayerInteraction interaction;

        [SerializeField] private GameObject flashlight;
        private bool _flashlightOn = true;

        [SerializeField] private GameObject crucifix;
        [SerializeField] private float crucifixCooldown;
        [SerializeField] private float crucifixTime;
        public bool crucifixAvailable = true;

        private void Awake()
        {
            movement = GetComponent<FpsMovement>();
            interaction = GetComponent<PlayerInteraction>();
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
            crucifixAvailable = false;
            crucifix.SetActive(true);

            yield return new WaitForSeconds(crucifixTime);
            
            crucifix.SetActive(false);
            StartCoroutine(CrucifixCooldown());
        }

        private IEnumerator CrucifixCooldown()
        {
            yield return new WaitForSeconds(crucifixCooldown);
            crucifixAvailable = true;
        }
    }
}
