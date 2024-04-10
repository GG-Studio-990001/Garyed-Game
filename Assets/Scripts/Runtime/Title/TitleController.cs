using Runtime.Common.View;
using Runtime.InGameSystem;
using UnityEngine;
using Sound = Runtime.ETC.Sound;

namespace Runtime.CH1.Title
{
    public class TitleController : MonoBehaviour
    {
        [SerializeField]
        private SceneSystem _sceneSystem;
        [SerializeField]
        private SettingsUIView _settingsUIView;

        private void Awake()
        {
            ToMain();
            SetSettingUI();
        }

        private void Start()
        {
            Managers.Sound.Play(Sound.BGM, "Title_BGM_CH1_02");
        }

        private void ToMain()
        {
            Managers.Data.GameOverControls.UI.Enable();
            Managers.Data.GameOverControls.UI.DialogueInput.performed += _ =>
            {
                _sceneSystem.LoadScene("Main");
            };
        }

        private void SetSettingUI()
        {
            Managers.Data.GameOverControls.UI.Enable();
            Managers.Data.GameOverControls.UI.GameSetting.performed += _ =>
            {
                _settingsUIView.GameSettingToggle();
            };
        }
    }
}