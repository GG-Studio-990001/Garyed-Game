using Runtime.Common.View;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sound = Runtime.ETC.Sound;

namespace Runtime.CH1.Title
{
    public class TitleController : MonoBehaviour
    {
        [SerializeField] 
        private SettingsUIView settingsUIView;

        private void Start()
        {
            Managers.Data.InGameKeyBinder.GameControlReset();
            
            Managers.Sound.Play(Sound.BGM, "Title_BGM_CH1_02");
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