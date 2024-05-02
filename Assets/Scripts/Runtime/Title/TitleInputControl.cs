using Runtime.Common.View;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleInputControl : MonoBehaviour
{
    [SerializeField]
    private SettingsUIView _settingsUIView;

    private void Awake()
    {
        TitleKeyBinding();
        CH1UIKeyBinding();
    }

    private void LoadMainScene(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            SceneManager.LoadScene("Main");
            TitleKeyUnbinding();
            CH1UIKeyUnbinding();
        }
    }

    private void TitleKeyBinding()
    {
        Managers.Data.GameOverControls.UI.Enable();
        Managers.Data.GameOverControls.UI.DialogueInput.performed += LoadMainScene;
    }

    private void TitleKeyUnbinding()
    {
        Managers.Data.GameOverControls.UI.Disable();
        Managers.Data.GameOverControls.UI.DialogueInput.performed -= LoadMainScene;
    }

    private void SetSettingUI(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _settingsUIView.GameSettingToggle();
        }
    }

    private void CH1UIKeyBinding()
    {
        Managers.Data.GameOverControls.UI.Enable();
        Managers.Data.GameOverControls.UI.GameSetting.performed += SetSettingUI;
    }

    private void CH1UIKeyUnbinding()
    {
        Managers.Data.GameOverControls.UI.Disable();
        Managers.Data.GameOverControls.UI.GameSetting.performed -= SetSettingUI;
    }
}