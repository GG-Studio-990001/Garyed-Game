using Runtime.Common.Presentation;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Runtime.Common.View
{
    public class SettingsUIView : MonoBehaviour
    {
        [field:SerializeField] private Slider musicVolumeSlider;
        [field:SerializeField] private Slider sfxVolumeSlider;
        [field:SerializeField] private Button exitButton;

        public UnityEvent onSettingUiEnable; 
        public UnityEvent onSettingUiDisable;

        
        private SettingsUIPresenter _presenter;
        
        private void Start()
        {
            _presenter = PresenterFactory.CreateSettingsUIPresenter(this);
        }

        private void OnEnable()
        {
            onSettingUiEnable?.Invoke();
        }
        
        private void OnDisable()
        {
            onSettingUiDisable?.Invoke();
        }

        public void SetMusicSlider(Slider slider)
        {
            musicVolumeSlider = slider;
        }
        
        public void SetSfxSlider(Slider slider)
        {
            sfxVolumeSlider = slider;
        }
        
        public void OnMusicSliderValueChanged(UnityAction<float> onValueChanged)
        {
            musicVolumeSlider.onValueChanged.AddListener(onValueChanged);
        }
        
        public void OnSfxSliderValueChanged(UnityAction<float> onValueChanged)
        {
            sfxVolumeSlider.onValueChanged.AddListener(onValueChanged);
        }
        
        public void SetExitButton(Button button)
        {
            exitButton = button;
        }
        
        public void OnExitButtonClicked(UnityAction onClick)
        {
            exitButton.onClick.AddListener(onClick);
        }
        
        public void SetViewMusicVolume(float volume)
        {
            musicVolumeSlider.value = volume;
        }
        
        public void SetViewSfxVolume(float volume)
        {
            sfxVolumeSlider.value = volume;
        }
    }
}