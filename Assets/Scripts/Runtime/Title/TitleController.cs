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
        private GameOverControls controls;

        private void Awake()
        {
            SetController();
        }

        private void Start()
        {
            Managers.Sound.Play(Sound.BGM, "Title_BGM_CH1_02");
        }

        private void SetController()
        {
            controls = new GameOverControls();
            controls.UI.Enable();

            controls.UI.GameSetting.performed += _ => _settingsUIView.GameSettingToggle();
            controls.UI.DialogueInput.performed += _ => _sceneSystem.LoadScene("Main");
            controls.UI.DialogueInput.canceled += _ => controls.Dispose();
        }
    }
}