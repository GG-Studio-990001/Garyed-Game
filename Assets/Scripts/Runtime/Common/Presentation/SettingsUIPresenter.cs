using Runtime.Common.Domain;
using Runtime.Common.View;
using UnityEngine;

namespace Runtime.Common.Presentation
{
    public class SettingsUIPresenter
    {
        private readonly SettingsUIView _settingsUIView;
        private readonly SettingsData _settingsData;
        private readonly AudioSource _backgroundAudioSource;
        private readonly AudioSource _effectAudioSource;
        
        public SettingsData SettingsData => _settingsData;

        public SettingsUIPresenter(SettingsUIView settingsUIView, SettingsData settingsData)
        {
            _settingsUIView = settingsUIView;
            _settingsData = settingsData;
            
            _settingsUIView.musicVolumeSlider.onValueChanged.AddListener(OnSliderMusicValueChanged);
            _settingsUIView.sfxVolumeSlider.onValueChanged.AddListener(OnSliderSfxValueChanged);
            
            _backgroundAudioSource = GameObject.Find("BackgroundAudioSource").GetComponent<AudioSource>();
            _effectAudioSource = GameObject.Find("EffectAudioSource").GetComponent<AudioSource>();
            
            if (_backgroundAudioSource == null)
            {
                Debug.LogError("Cannot find BackgroundAudioSource");
            }
            
            if (_effectAudioSource == null)
            {
                Debug.LogError("Cannot find EffectAudioSource");
            }
            
            SetMusicVolume(_settingsData.MusicVolume);
            SetSfxVolume(_settingsData.SfxVolume);
        }
        
        private void OnSliderMusicValueChanged(float value)
        {
            SetMusicVolume(value);
        }
        
        private void OnSliderSfxValueChanged(float value)
        {
            SetSfxVolume(value);
        }
        
        public void SetMusicVolume(float volume)
        {
            _settingsData.MusicVolume = volume;
            _settingsUIView.SetViewMusicVolume(_settingsData.MusicVolume);
            
            _backgroundAudioSource.volume = _settingsData.MusicVolume;
        }
        
        public void SetSfxVolume(float volume)
        {
            _settingsData.SfxVolume = volume;
            _settingsUIView.SetViewSfxVolume(_settingsData.SfxVolume);
            
            _effectAudioSource.volume = _settingsData.SfxVolume;
        }
    }
}