﻿using System;
using Code.Player;
using Code.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Code.Level
{
    public class LevelManager : MonoBehaviour
    {
        [NonSerialized] public PlayerController player;
        private UIManager _uiManager;
        public bool WinningConditionsMet => CollectablesInPossession >= collectablesNeeded;

        [SerializeField] private int collectablesNeeded;

        [SerializeField] private int collectablesInPossession;
        [SerializeField] private string nextSceneName;
        public int CollectablesInPossession
        {
            get => collectablesInPossession;
            set
            {
                collectablesInPossession = value;
            }
        }

        private void Start()
        {
            player = FindObjectOfType<PlayerController>();
            _uiManager = FindObjectOfType<UIManager>();
            UpdateUI();
        }

        public void AddCollectable()
        {
            CollectablesInPossession++;
            UpdateUI();
        }

        public void ResetLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void EndLevel()
        {
            if (nextSceneName == "")
            {
                Debug.LogError("No next scene set");
                return;
            }
            SceneManager.LoadScene(nextSceneName);
        }

        private void UpdateUI()
        {
            _uiManager.SetCollectableText(collectablesInPossession, collectablesNeeded);
        }
    }
}