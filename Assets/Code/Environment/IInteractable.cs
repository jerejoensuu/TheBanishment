using UnityEngine;

namespace Code.Environment
{
    public interface IInteractable
    {
        bool IsCurrentlyInteractable();

        string LookAtText();
        void EnableHoverEffect();
        void DisableHoverEffect();

        void Interact();
    }
}