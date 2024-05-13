using Runtime.Common.Presentation;
using Runtime.Event;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Common.View
{
    public class SettingsUIView : MonoBehaviour
    {
        [field:SerializeField] public GameObject SettingUIObject { get; set; }
        [field:SerializeField] public Slider BgmVolumeSlider { get; set; }
        [field:SerializeField] public Slider SfxVolumeSlider { get; set; }
        [field:SerializeField] public Button ExitButton { get; set; }
        [field:SerializeField] public Button GameExitButton { get; set; }
        
        private SettingsUIPresenter _presenter;
        
        public event Action OnSettingsOpen;
        public event Action OnSettingsClose;
        
        private void Start()
        {
            _presenter = PresenterFactory.CreateSettingsUIPresenter(this);
        }
        
        public void GameSettingToggle()
        {
            if (SettingUIObject.activeSelf)
            {
                OnSettingsClose?.Invoke();
                SettingsEvent.ToggleSettings(false);
                // Managers.Data.SaveGame(); // 볼륨 해야되는데..
            }
            else
            {
                OnSettingsOpen?.Invoke();
                SettingsEvent.ToggleSettings(true);
            }
            
            SettingUIObject.SetActive(!SettingUIObject.activeSelf);
        }
    }
}