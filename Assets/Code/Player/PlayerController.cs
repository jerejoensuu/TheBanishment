using System;
using UnityEngine;

namespace Code.Player
{
    [RequireComponent(typeof(FpsMovement))]
    public class PlayerController : MonoBehaviour
    {
        [HideInInspector] public FpsMovement movement;

        private void Awake()
        {
            movement = GetComponent<FpsMovement>();
        }
    }
}
