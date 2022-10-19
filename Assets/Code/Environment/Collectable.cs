using Code.Level;
using UnityEngine;

namespace Code.Environment
{
    public class Collectable : MonoBehaviour, IInteractable
    {
        private LevelManager _levelManager;
        private bool _collected;

        private void Start()
        {
            _levelManager = FindObjectOfType<LevelManager>();
        }

        private void Collect()
        {
            _levelManager.AddCollectable();
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

        public void Interact()
        {
            Collect();
        }
    }
}