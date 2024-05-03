using Runtime.Common.View;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleInputControl
{
    private SettingsUIView _settingsUIView;
    
    public TitleInputControl(SettingsUIView settingsUIView)
    {
        _settingsUIView = settingsUIView;
    }

    // private void TitleKeyBinding()
    // {
    //     Managers.Data.GameOverControls.UI.Enable();
    //     Managers.Data.GameOverControls.UI.DialogueInput.performed += _ => LoadMainScene();
    // }
    //
    // private void TitleKeyUnbinding()
    // {
    //     Managers.Data.GameOverControls.UI.Disable();
    //     Managers.Data.GameOverControls.UI.DialogueInput.performed -= _ => LoadMainScene();
    // }
    //
    // public void TitleSettingUIKeyBinding()
    // {
    //     Managers.Data.GameOverControls.UI.Enable();
    //     Managers.Data.GameOverControls.UI.GameSetting.performed += SetSettingUI; //  _settingsUIView.GameSettingToggle();
    // }
    //
    // public void TitleSettingUIUnbinding()
    // {
    //     Managers.Data.GameOverControls.UI.Disable();
    //     Managers.Data.GameOverControls.UI.GameSetting.performed -= SetSettingUI;
    // }
}