using Runtime.Common.View;
using UnityEngine;

namespace Runtime.InGameSystem
{
    public class ControlSystem : MonoBehaviour
    {
        public GameOverControls controls;
        [SerializeField]
        private SettingsUIView _settingsUIView;

        // 접속게임 컨트롤을 따로 관리하기 위한 스크립트

        private void Awake()
        {
            InitNewControl();
            SetSettingUI();
        }

        private void InitNewControl()
        {
            Managers.Data.GameOverControls.Disable();

            controls = new GameOverControls();
            controls.UI.Enable();
        }

        private void SetSettingUI()
        {
            controls.UI.GameSetting.performed += _ =>
            {
                _settingsUIView.GameSettingToggle();
                Time.timeScale = (Time.timeScale == 0 ? 1 : 0);
            };
        }

        public void ExitNewControl()
        {
            controls.Dispose();

            Managers.Data.GameOverControls.Enable();
        }
    }
}