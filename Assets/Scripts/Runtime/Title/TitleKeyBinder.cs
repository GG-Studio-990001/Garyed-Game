using Runtime.Common.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.CH1.Title
{
    public class TitleKeyBinder : MonoBehaviour
    {
        [SerializeField]
        private SettingsUIView _settingsUIView;
        [SerializeField]
        private GameObject _timeline;

        private void Start()
        {
            InitKeyBinding();
        }

        private void InitKeyBinding()
        {
            Managers.Data.InGameKeyBinder.GameControlReset();
            Managers.Data.InGameKeyBinder.TitleKeyBinding(this);
        }

        public void ActiveTimeline()
        {
            _timeline.SetActive(true);
        }

        public void SetSettingUI()
        {
            _settingsUIView.GameSettingToggle();
        }
    }
}