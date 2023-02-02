using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{
    [SerializeField] private float soundEffectsVolume;
    [SerializeField] private float musicVolume;

    [SerializeField] private Slider sfSlider;
    [SerializeField] private Slider musicSlider;

    private void Update()
    {
        soundEffectsVolume = sfSlider.value;
        musicVolume = musicSlider.value;
    }

    private void OnEnable()
    {
        LoadOptions();
    }

    private void OnDisable()
    {
        SaveOptions();
    }
    
    private void SaveOptions()
    {
        PlayerPrefs.SetFloat("sfvolume", soundEffectsVolume);
        PlayerPrefs.SetFloat("musicvolume", musicVolume);
    }

    private void LoadOptions()
    {
        soundEffectsVolume = PlayerPrefs.GetFloat("sfvolume");
        musicVolume = PlayerPrefs.GetFloat("musicvolume");
        
        sfSlider.value = soundEffectsVolume;
        musicSlider.value = musicVolume;
    }
}
