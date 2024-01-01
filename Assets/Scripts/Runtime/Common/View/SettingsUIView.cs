using Runtime.Common.Presentation;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Runtime.Common.View
{
    public class SettingsUIView : MonoBehaviour
    {
        [field:SerializeField] public Slider MusicVolumeSlider { get; set; }
        [field:SerializeField] public Slider SfxVolumeSlider { get; set; }
        [field:SerializeField] public Button ExitButton { get; set; }

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
    }
}