using System;
using Code.Player;
using Code.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Code.Level
{
    public class LevelManager : MonoBehaviour
    {
        private PlayerController _player;
        private UIManager _uiManager;
        public bool WinningConditionsMet => CollectablesInPossession >= collectablesNeeded;

        [SerializeField] private int collectablesNeeded;

        [SerializeField] private int collectablesInPossession;
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
            _player = FindObjectOfType<PlayerController>();
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
            ResetLevel();
        }

        private void UpdateUI()
        {
            _uiManager.SetCollectableText(collectablesInPossession, collectablesNeeded);
        }
    }
}