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

        public SettingsUIPresenter(SettingsUIView settingsUIView)
        {
            _settingsUIView = settingsUIView;
            
            _settingsUIView.MusicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            _settingsUIView.SfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
            
            SetMusicVolume(Managers.Data.MusicVolume);
            SetSfxVolume(Managers.Data.SfxVolume);
            
            _settingsUIView.GameExitButton.onClick.AddListener(OnGameExitButtonClicked);
            _settingsUIView.ExitButton.onClick.AddListener(_settingsUIView.GameSettingToggle);
        }
        
        private void SetMusicVolume(float volume)
        {
            Managers.Data.MusicVolume = volume;
            _settingsUIView.MusicVolumeSlider.value = volume;
        }
        
        private void SetSfxVolume(float volume)
        {
            Managers.Data.SfxVolume = volume;
            _settingsUIView.SfxVolumeSlider.value = volume;
        }
        
        private void OnGameExitButtonClicked()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();

            //Managers.Sound.Play()
        }
    }
}