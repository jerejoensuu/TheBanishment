using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Code.Player
{
    public class PlayerAudio : MonoBehaviour
    {
        public EventReference eventReference;
        private EventInstance _audio;

        private void Start()
        {
            _audio = RuntimeManager.CreateInstance(eventReference);
            RuntimeManager.AttachInstanceToGameObject(_audio, GetComponent<Transform>(), GetComponent<Rigidbody>());
        }
        
        public void Test()
        {
            Debug.Log("Click");
            _audio = RuntimeManager.CreateInstance(eventReference);
            RuntimeManager.AttachInstanceToGameObject(_audio, GetComponent<Transform>(), GetComponent<Rigidbody>());
            _audio.start();
            _audio.release();
        }
    }
}