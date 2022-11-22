using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Code.Enemy
{
    public class EnemyAnimationManager : MonoBehaviour
    {
        public EventReference footstep;
        private EventInstance _audio;

        public void PlayFootstepAudio()
        {
            PlayAudio(footstep);
        }
        
        private void PlayAudio(EventReference eventReference)
        {
            Debug.Log("Step");
            _audio = RuntimeManager.CreateInstance(eventReference);
            RuntimeManager.AttachInstanceToGameObject(_audio, GetComponent<Transform>(), GetComponent<Rigidbody>());
            _audio.start();
            _audio.release();
        }
    }
}
