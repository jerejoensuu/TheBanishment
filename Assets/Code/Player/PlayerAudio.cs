using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Code.Player
{
    public class PlayerAudio : MonoBehaviour
    {
        public EventReference eventReference;
        private EventInstance _audio;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float pickupVolume = 0.2f;

        private void Start()
        {
            _audio = RuntimeManager.CreateInstance(eventReference);
            RuntimeManager.AttachInstanceToGameObject(_audio, GetComponent<Transform>(), GetComponent<Rigidbody>());
        }
        
        public void PlayPickupAudio()
        {
            audioSource.volume = PlayerPrefs.GetFloat("sfvolume") * pickupVolume;
            audioSource.Play();
        }
    }
}