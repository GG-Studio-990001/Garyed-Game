using Runtime.Common.Presentation;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Common.View
{
    public class SettingsUIView : MonoBehaviour
    {
        [field:SerializeField] public GameObject SettingUIObject { get; set; }
        [field:SerializeField] public Slider MusicVolumeSlider { get; set; }
        [field:SerializeField] public Slider SfxVolumeSlider { get; set; }
        [field:SerializeField] public Button ExitButton { get; set; }
        [field:SerializeField] public Button GameExitButton { get; set; }
        
        private SettingsUIPresenter _presenter;
        
        public Action OnSettingsOpen;
        public Action OnSettingsClose;
        
        private void Start()
        {
            _presenter = PresenterFactory.CreateSettingsUIPresenter(this);
        }
        
        public void GameSettingToggle()
        {
            if (SettingUIObject.activeSelf)
            {
                OnSettingsClose?.Invoke();
            }
            else
            {
                OnSettingsOpen?.Invoke();
            }
            
            SettingUIObject.SetActive(!SettingUIObject.activeSelf);
        }
    }
}