using Runtime.CH1.Pacmom;
using Runtime.Common.View;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PMKeyBinder : MonoBehaviour
    {
        [SerializeField]
        private SettingsUIView settingsUIView;
        [SerializeField]
        private Rapley rapley;

        private void Start()
        {
            InitKeyBinding();
        }

        private void InitKeyBinding()
        {
            Managers.Data.InGameKeyBinder.GameControlReset();
            Managers.Data.InGameKeyBinder.PMKeyBinding(this, rapley);
        }

        public void SetSettingUI()
        {
            // Esc에 부착
            settingsUIView.GameSettingToggle();
            TimeScaleToggle();

            // X 버튼에는 직접 부착함
            // AddListner했더니 여러번 호출됨...
            // settingsUIView.ExitButton.onClick.AddListener(TimeScaleToggle);
        }

        public void TimeScaleToggle()
        {
            Time.timeScale = (Time.timeScale == 0 ? 1 : 0);
            Debug.Log("Time.timeScale " + Time.timeScale);
        }
    }
}