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
        }
        
        public float GetViewMusicVolume()
        {
            return musicVolumeSlider.value;
        }
        
        public float GetViewSfxVolume()
        {
            return sfxVolumeSlider.value;
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