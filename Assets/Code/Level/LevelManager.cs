using System;
using Code.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Code.Level
{
    public class LevelManager : MonoBehaviour
    {
        private PlayerController _player;
        public bool WinningConditionsMet { get; private set; }

        [SerializeField] private int collectablesNeeded;

        [SerializeField] private int collectablesInPossession;

        [SerializeField] private TextMeshProUGUI candleText; 

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
            UpdateText();
        }

        public void AddCollectable()
        {
            CollectablesInPossession++;
            UpdateText();
        }

        public void ResetLevel()
        {
            SceneManager.LoadScene("MockupLevel");
        }

        public void EndLevel()
        {
            ResetLevel();
        }

        private void UpdateText()
        {
            candleText.text = CollectablesInPossession.ToString() + "/" + collectablesNeeded.ToString();
        }
    }
}