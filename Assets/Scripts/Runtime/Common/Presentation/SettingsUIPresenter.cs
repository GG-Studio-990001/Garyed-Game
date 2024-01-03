using Runtime.Common.View;
using Runtime.Data;
using Runtime.Data.Original;
using Runtime.InGameSystem;
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
        
        private readonly string _backgroundAudioSourceName = "BackgroundAudioSource";
        private readonly string _effectAudioSourceName = "EffectAudioSource";

        public SettingsUIPresenter(SettingsUIView settingsUIView, SettingsData settingsData)
        {
            _settingsUIView = settingsUIView;
            _settingsData = settingsData;
            
            _settingsUIView.MusicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            _settingsUIView.SfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
            
            _backgroundAudioSource = GameObject.Find(_backgroundAudioSourceName).GetComponent<AudioSource>();
            _effectAudioSource = GameObject.Find(_effectAudioSourceName).GetComponent<AudioSource>();
            
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
            
            _settingsUIView.GameExitButton.onClick.AddListener(OnGameExitButtonClicked);
            _settingsUIView.ExitButton.onClick.AddListener(OnExitButtonClicked);
        }
        
        private void SetMusicVolume(float volume)
        {
            _settingsData.MusicVolume = volume;
            _settingsUIView.MusicVolumeSlider.value = _settingsData.MusicVolume;
            
            _backgroundAudioSource.volume = _settingsData.MusicVolume;
        }
        
        private void SetSfxVolume(float volume)
        {
            _settingsData.SfxVolume = volume;
            _settingsUIView.SfxVolumeSlider.value = _settingsData.SfxVolume;
            
            _effectAudioSource.volume = _settingsData.SfxVolume;
        }
        
        private void OnGameExitButtonClicked()
        {
            DataProviderManager.Instance.SettingsDataProvider.Set(_settingsData);
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
        
        private void OnExitButtonClicked()
        {
            DataProviderManager.Instance.SettingsDataProvider.Set(_settingsData);
            
            _settingsUIView.gameObject.SetActive(false);
        }
    }
}