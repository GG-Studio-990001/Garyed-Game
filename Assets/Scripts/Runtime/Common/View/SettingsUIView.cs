using Runtime.Common.Presentation;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Common.View
{
    public class SettingsUIView : MonoBehaviour
    {
        [field:SerializeField] public Slider musicVolumeSlider;
        [field:SerializeField] public Slider sfxVolumeSlider;
        
        private SettingsUIPresenter _presenter;
        
        private void Start()
        {
            _presenter = PresenterFactory.CreateSettingsUIPresenter(this);

            musicVolumeSlider.onValueChanged.AddListener(OnSliderMusicValueChanged);
            sfxVolumeSlider.onValueChanged.AddListener(OnSliderSfxValueChanged);
        }

        public void SetViewMusicVolume(float volume)
        {
            musicVolumeSlider.value = volume;
        }
        
        public void SetViewSfxVolume(float volume)
        {
            sfxVolumeSlider.value = volume;
        }
        
        public float GetViewMusicVolume()
        {
            return musicVolumeSlider.value;
        }
        
        public float GetViewSfxVolume()
        {
            return sfxVolumeSlider.value;
        }
        
        private void OnSliderMusicValueChanged(float value)
        {
            _presenter.SetMusicVolume(value);
        }
        
        private void OnSliderSfxValueChanged(float value)
        {
            _presenter.SetSfxVolume(value);
        }
    }
}