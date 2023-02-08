using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Code.Enemy
{
    public class EnemyAnimationManager : MonoBehaviour
    {
        public EventReference footstep;
        private EventInstance _audio;
        [SerializeField] private float footstepVolume = 0.3f;

        public void PlayFootstepAudio()
        {
            PlayAudio(footstep);
        }
        
        private void PlayAudio(EventReference eventReference)
        {
            _audio = RuntimeManager.CreateInstance(eventReference);
            _audio.setVolume(PlayerPrefs.GetFloat("sfvolume") * footstepVolume);
            RuntimeManager.AttachInstanceToGameObject(_audio, GetComponent<Transform>(), GetComponent<Rigidbody>());
            _audio.start();
            _audio.release();
        }
    }
}
