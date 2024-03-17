using Runtime.Common.View;
using Runtime.Data.Original;
using Runtime.InGameSystem;
using Runtime.Interface;
using UnityEngine;

namespace Runtime.Common.Presentation
{
    public class SettingsUIPresenter
    {
        private readonly SettingsUIView _settingsUIView;
        private readonly SettingsData _settingsData;
        
        public SettingsData SettingsData => _settingsData;
        
        private readonly string _backgroundAudioSourceName = "BackgroundAudioSource";
        private readonly string _effectAudioSourceName = "EffectAudioSource";
        
        private IProvider<ControlsData> ControlsDataProvider => DataProviderManager.Instance.ControlsDataProvider;
        private IProvider<SettingsData> SettingsDataProvider => DataProviderManager.Instance.SettingsDataProvider;

        public SettingsUIPresenter(SettingsUIView settingsUIView, SettingsData settingsData)
        {
            _settingsUIView = settingsUIView;
            _settingsData = settingsData;
            
            _settingsUIView.MusicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            _settingsUIView.SfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
            
            SetMusicVolume(_settingsData.MusicVolume);
            SetSfxVolume(_settingsData.SfxVolume);
            
            _settingsUIView.GameExitButton.onClick.AddListener(OnGameExitButtonClicked);
            _settingsUIView.ExitButton.onClick.AddListener(_settingsUIView.GameSettingToggle);
        }
        
        private void SetMusicVolume(float volume)
        {
            _settingsData.MusicVolume = volume;
            _settingsUIView.MusicVolumeSlider.value = _settingsData.MusicVolume;
            
            Managers.Sound.BGM.volume = _settingsData.MusicVolume;
        }
        
        private void SetSfxVolume(float volume)
        {
            _settingsData.SfxVolume = volume;
            _settingsUIView.SfxVolumeSlider.value = _settingsData.SfxVolume;
            
            Managers.Sound.Effect.volume = _settingsData.SfxVolume;
        }
        
        private void OnGameExitButtonClicked()
        {
            SettingsDataProvider.Set(_settingsData);
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }
}