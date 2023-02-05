using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Code.Level;

public class AudioOptions : MonoBehaviour
{
    [SerializeField] private float soundEffectsVolume;
    [SerializeField] private float musicVolume;

    [SerializeField] private Slider sfSlider;
    [SerializeField] private Slider musicSlider;

    [SerializeField] private MusicManager musicManager;

    public void UpdateValues()
    {
        soundEffectsVolume = sfSlider.value;
        musicVolume = musicSlider.value;
        SaveOptions();
        
        SetMusicManagerVolume();
    }

    private void OnEnable()
    {
        LoadMusicVolume();
        LoadSFVolume();
        SetMusicManagerVolume();
    }

    private void OnDisable()
    {
        SaveOptions();
        SetMusicManagerVolume();
    }
    
    private void SaveOptions()
    {
        PlayerPrefs.SetFloat("sfvolume", soundEffectsVolume);
        PlayerPrefs.SetFloat("musicvolume", musicVolume);
    }

    private void LoadMusicVolume()
    {
        musicVolume = PlayerPrefs.GetFloat("musicvolume");
        musicSlider.value = musicVolume;
    }

    private void LoadSFVolume()
    {
        soundEffectsVolume = PlayerPrefs.GetFloat("sfvolume");
        sfSlider.value = soundEffectsVolume;
    }

    private void SetMusicManagerVolume()
    {
        if (musicManager == null)
        {
            return;
        } else 
        {
            musicManager.GetVolumeOptions();
        }
    }
}
