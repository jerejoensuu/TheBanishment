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
            _audio = RuntimeManager.CreateInstance(eventReference);
            RuntimeManager.AttachInstanceToGameObject(_audio, GetComponent<Transform>(), GetComponent<Rigidbody>());
            _audio.setVolume(0.7f);
            _audio.start();
            _audio.release();
        }
    }
}
