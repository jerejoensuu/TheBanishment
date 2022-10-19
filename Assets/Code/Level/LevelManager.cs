using System;
using Code.Player;
using UnityEngine;

namespace Code.Level
{
    public class LevelManager : MonoBehaviour
    {
        private PlayerController _player;
        public bool WinningConditionsMet { get; private set; }

        [SerializeField] private int collectablesNeeded;

        [SerializeField] private int collectablesInPossession;

        public int CollectablesInPossession
        {
            get => collectablesInPossession;
            set
            {
                collectablesInPossession = value;
                WinningConditionsMet = collectablesInPossession >= collectablesNeeded;
            }
        }

        private void Start()
        {
            _player = FindObjectOfType<PlayerController>();
            collectablesInPossession = 0;
        }

        public void AddCollectable()
        {
            CollectablesInPossession++;
        }
    }
}