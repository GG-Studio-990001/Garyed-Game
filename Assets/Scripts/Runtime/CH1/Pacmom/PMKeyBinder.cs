using Runtime.CH1.Pacmom;
using Runtime.Common.View;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Pacmom
{
    public class PMKeyBinder : MonoBehaviour
    {
        [SerializeField]
        private SettingsUIView _settingsUIView;
        [SerializeField]
        private LineView _lineView;
        [SerializeField]
        private Rapley _rapley;

        private void Start()
        {
            InitKeyBinding();
        }

        private void InitKeyBinding()
        {
            Managers.Data.InGameKeyBinder.GameControlReset();
            Managers.Data.InGameKeyBinder.PMKeyBinding(this, _lineView, _rapley);
        }

        public void SetSettingUI()
        {
            // Esc에 부착
            _settingsUIView.GameSettingToggle();
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