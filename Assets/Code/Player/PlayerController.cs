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

        
    }
}
