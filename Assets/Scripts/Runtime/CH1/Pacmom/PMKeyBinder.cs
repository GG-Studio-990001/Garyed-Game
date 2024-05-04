using Runtime.CH1.Pacmom;
using Runtime.Common.View;
using UnityEngine;

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
        settingsUIView.GameSettingToggle();
        Time.timeScale = (Time.timeScale == 0 ? 1 : 0);
    }
}