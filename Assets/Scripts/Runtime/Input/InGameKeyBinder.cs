using Runtime.CH1.Main.Player;
using Runtime.CH1.Title;
using Runtime.Common.View;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Runtime.Input
{
    // 현재 플랫폼에 맞게 키 바인딩을 설정하는 클래스
    public class InGameKeyBinder
    {
        private GameOverControls _gameOverControls;
        
        private int _playerInputEnableStack;
        
        public InGameKeyBinder(GameOverControls gameOverControls)
        {
            _gameOverControls = gameOverControls;
        }
        
        // Title Key Binding
        public void TitleKeyBinding(TitleController titleControl)
        {
            _gameOverControls.UI.Enable();
            _gameOverControls.UI.DialogueInput.performed += _ => titleControl.LoadMainScene();
            _gameOverControls.UI.GameSetting.performed += _ => titleControl.SetSettingUI();
        }

        // CH1 Key Bind
        public void CH1PlayerKeyBinding(TopDownPlayer player)
        {
            _gameOverControls.Player.Enable();
            _gameOverControls.Player.Move.performed += player.OnMove;
            _gameOverControls.Player.Move.started += player.OnMove;
            _gameOverControls.Player.Move.canceled += player.OnMove;
            _gameOverControls.Player.Interaction.performed += _ => player.OnInteraction();
        }
        
        public void CH1UIKeyBinding(SettingsUIView settingsUIView)
        {
            _gameOverControls.UI.Enable();
            _gameOverControls.UI.GameSetting.performed += _ => settingsUIView.GameSettingToggle();
        }
        
        // ETC
        public void GameControlReset()
        {
            _gameOverControls.Dispose();
            _gameOverControls = new GameOverControls();

            _playerInputEnableStack = 0;
        }

        public void PlayerInputDisable()
        {
            _playerInputEnableStack++;
            
            if (_playerInputEnableStack > 0)
            {
                _gameOverControls.Player.Disable();
            }
        }
        
        public void PlayerInputEnable()
        {
            if (_playerInputEnableStack > 0)
            {
                _playerInputEnableStack--;
            }
            
            if (_playerInputEnableStack == 0)
                _gameOverControls.Player.Enable();
        }
    }
}