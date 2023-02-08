using System;
using UnityEngine;

namespace Code.Level
{
    public class MusicManager : MonoBehaviour
    {
        public MusicState musicState;

        [SerializeField] private AudioSource ambientSource;
        [SerializeField] private AudioSource chaseSource;

        private float globalMusicVolume;

        private float ambientVolume;
        private float chaseVolume = 0f;
        
        [SerializeField] private float maxAmbientVolume;
        [SerializeField] private float maxChaseVolume;
        
        public enum MusicState
        {
            Ambient,
            Chase
        }
        
        private void Start()
        {
            GetVolumeOptions();

            ambientSource.volume = ambientVolume = maxAmbientVolume;
            chaseSource.volume = chaseVolume;
        }

        private void Update()
        {
            switch (musicState)
            {
                case MusicState.Ambient:
                    ambientVolume = Mathf.Lerp(ambientVolume, maxAmbientVolume, Time.deltaTime);
                    chaseVolume = Mathf.Lerp(chaseVolume, 0f, Time.deltaTime);
                    break;
                case MusicState.Chase:
                    ambientVolume = Mathf.Lerp(ambientVolume, 0f, Time.deltaTime * 5);
                    chaseVolume = Mathf.Lerp(chaseVolume, maxChaseVolume, Time.deltaTime * 5);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ambientSource.volume = ambientVolume * globalMusicVolume;
            chaseSource.volume = chaseVolume * globalMusicVolume;

            if (chaseSource.volume < 0.01f)
                chaseSource.Stop();
            else if (!chaseSource.isPlaying)
                chaseSource.Play();
        }

        public void GetVolumeOptions()
        {
            globalMusicVolume = PlayerPrefs.GetFloat("musicvolume");
        }
    }
}