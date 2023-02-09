using System;
using Code.Level;
using UnityEngine;

namespace Code.Environment
{
    public class AltarManager : MonoBehaviour, IInteractable
    {
        private LevelManager levelManager;
        [SerializeField] private GameObject candles;
        // [SerializeField] private int candlesNeeded;
        // [SerializeField] private int candlesPlaced;

        private void Start()
        {
            levelManager = FindObjectOfType<LevelManager>();
        }

        public bool IsCurrentlyInteractable()
        {
            return levelManager.WinningConditionsMet && !candles.activeSelf && !candles.activeSelf;
        }

        public string LookAtText()
        {
            return "Place candles";
        }

        public void EnableHoverEffect() { }

        public void DisableHoverEffect() { }

        public void Interact()
        {
            candles.SetActive(true);
            if (PlayerPrefs.GetInt("levelProgress") == 1)
            {
                PlayerPrefs.SetInt("levelProgress", 2);
            }
            else if (PlayerPrefs.GetInt("levelProgress") == 3)
            {
                PlayerPrefs.SetInt("levelProgress", 2);
            }
            levelManager.EndLevel();
        }
    }
}
