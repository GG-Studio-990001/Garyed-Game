using Runtime.Common.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.CH1.Title
{
    public class TitleKeyBinder : MonoBehaviour
    {
        [SerializeField]
        private SettingsUIView settingsUIView;

        private void Start()
        {
            InitKeyBinding();
        }

        private void InitKeyBinding()
        {
            Managers.Data.InGameKeyBinder.GameControlReset();
            Managers.Data.InGameKeyBinder.TitleKeyBinding(this);
        }

        public void LoadMainScene()
        {
            SceneManager.LoadScene("Main");
        }

        public void SetSettingUI()
        {
            settingsUIView.GameSettingToggle();
        }
    }
}