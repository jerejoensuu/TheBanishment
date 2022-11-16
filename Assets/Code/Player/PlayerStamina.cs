using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Player
{
    public class PlayerStamina : MonoBehaviour
    {
        [SerializeField] private FpsMovement fpsMov;

        [SerializeField] private Slider staminaBar;
        [SerializeField] private Image barFill;

        [Range(0,100)] private float stamina = 100;

        [SerializeField] private float staminaDrain;
        [SerializeField] private float staminaRegen;
        [SerializeField] private float cooldownTime;

        public bool outOfStamina = false;
        private bool tickDone = true;

        private void Update()
        {
            if (tickDone)
            {
                StartCoroutine(TickMeter());
            }
        }

        private IEnumerator TickMeter()
        {
            tickDone = false;

            yield return new WaitForSeconds(0.1f);
        
            if (fpsMov.running)
            {
                stamina -= staminaDrain;
            } 
            else if (!fpsMov.running)
            {
                stamina += staminaRegen;
            } 

            if (stamina <= 0)
            {
                StartCoroutine(StaminaCooldown());
            }

            stamina = Mathf.Clamp(stamina, 0, 100);

            staminaBar.value = stamina;

            tickDone = true;
        }

        private IEnumerator StaminaCooldown()
        {
            outOfStamina = true;
            barFill.color = Color.red;
            staminaRegen = 1;

            yield return new WaitForSeconds(cooldownTime);
            
            staminaRegen = 2;
            barFill.color = Color.green;
            outOfStamina = false;
        }
    }
}