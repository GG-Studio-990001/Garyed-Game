using Runtime.Common.View;
using UnityEngine;
using Sound = Runtime.ETC.Sound;

namespace Runtime.CH1.Title
{
    public class TitleController : MonoBehaviour
    {
        private void Awake()
        {
            SetEscape();
        }

        private void Start()
        {
            Managers.Sound.Play(Sound.BGM, "Title_BGM_01");
        }

        private void SetEscape()
        {
            Managers.Data.GameOverControls.UI.Enable();
            Managers.Data.GameOverControls.UI.GameSetting.performed += _ =>
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            };
        }
    }
}