using Code.Level;
using UnityEngine;
using Code.Player;

namespace Code.Environment
{
    public class Collectable : MonoBehaviour, IInteractable
    {
        private LevelManager _levelManager;
        private bool _collected;
        public Outline outline;

        [SerializeField] private PlayerController player;
        [SerializeField] private bool isCrucifix = false;

        private void Start()
        {
            _levelManager = FindObjectOfType<LevelManager>();
            DisableHoverEffect();
        }

        private void Collect()
        {
            if (!isCrucifix)
            {
                _levelManager.AddCollectable();
            } else 
            {
                player.AddCrucifix();
            }

            _collected = true;
            // TODO: Play sound before destroying
            Destroy(gameObject);
        }

        public bool IsCurrentlyInteractable()
        {
            return !_collected;
        }

        public string LookAtText()
        {
            return "Pick up";
        }

        public void EnableHoverEffect()
        {
            SetOutline(true);
        }

        public void DisableHoverEffect()
        {
            SetOutline(false);
        }
        
        private void SetOutline(bool active)
        {
            if (outline == null) return;
            outline.enabled = active;
        }

        public void Interact()
        {
            Collect();
            _levelManager.player.playerAudio.PlayPickupAudio();
        }
    }
}