using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    public static float _screenBrightness = 1;
    public static float _soundEffectsVolume = 0.5f;
    public static float _musicVolume = 0.5f;
    public static float _sensitivityValue = 0.5f;
    [Header("Slider Objects")]
    [SerializeField] Slider brightnessSliderObject;
    [SerializeField] Slider sfxSliderObject;
    [SerializeField] Slider musicSliderObject;
    [SerializeField] Slider sensSliderObject;
    
    
    // Start is called before the first frame update
    void Start()
    {
        ScreenBrightnessSlider(OptionsController._screenBrightness);
        brightnessSliderObject.value = OptionsController._screenBrightness;
        MusicVolumeSlider(OptionsController._musicVolume);
        musicSliderObject.value = OptionsController._musicVolume;
        SFXVolumeSlider(OptionsController._soundEffectsVolume);
        sfxSliderObject.value = OptionsController._soundEffectsVolume;
        SensitivitySlider(OptionsController._sensitivityValue);
        sensSliderObject.value = OptionsController._sensitivityValue;
    
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    public void ScreenBrightnessSlider(float value){
        _screenBrightness = value;
        
        Screen.brightness = _screenBrightness;

    }

    public void MusicVolumeSlider(float value){
        _musicVolume = value;
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in audioSources){
            if(a.tag == "Music"){
                a.volume = _musicVolume;
                
            }
        }
    }

    public void SFXVolumeSlider(float value){
        _soundEffectsVolume = value;
        AudioSource[] audioSources = Resources.FindObjectsOfTypeAll<AudioSource>();
        foreach (AudioSource a in audioSources){
            if(a.tag == "SFX"){
                a.volume = _soundEffectsVolume;
            }
        }

    }

    public void SensitivitySlider(float value){
        _sensitivityValue = value;
    }
}
