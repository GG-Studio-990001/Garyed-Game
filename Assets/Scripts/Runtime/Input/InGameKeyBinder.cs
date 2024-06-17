using Runtime.CH1.Main.Player;
using Runtime.CH1.Pacmom;
using Runtime.CH1.Title;
using Runtime.CH2.Main;
using Runtime.Common.View;
using Yarn.Unity;
using static PlasticPipe.PlasticProtocol.Messages.Serialization.ItemHandlerMessagesSerialization;

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
        
        // Title
        public void TitleKeyBinding(TitleKeyBinder keyBinder)
        {
            _gameOverControls.UI.Enable();
            _gameOverControls.UI.DialogueInput.performed += _ => keyBinder.ActiveTimeline();
            _gameOverControls.UI.GameSetting.performed += _ => keyBinder.SetSettingUI();
        }

        // Pacmom
        public void PMKeyBinding(PMKeyBinder keyBinder, LineView line, Rapley rapley )
        {
            _gameOverControls.UI.Enable();
            _gameOverControls.UI.GameSetting.performed += _ => keyBinder.SetSettingUI();
            _gameOverControls.UI.DialogueInput.performed += _ => line.OnContinueClicked();

            _gameOverControls.Player.Enable();
            _gameOverControls.Player.Move.performed += rapley.OnMove;
            _gameOverControls.Player.Move.started += rapley.OnMove;
            _gameOverControls.Player.Move.canceled += rapley.OnMove;
        }

        // CH1
        public void CH1PlayerKeyBinding(TopDownPlayer player)
        {
            _gameOverControls.Player.Enable();
            _gameOverControls.Player.Move.performed += player.OnMove;
            _gameOverControls.Player.Move.started += player.OnMove;
            _gameOverControls.Player.Move.canceled += player.OnMove;
            _gameOverControls.Player.Interaction.performed += _ => player.OnInteraction();
        }
        
        public void CH1UIKeyBinding(SettingsUIView settingsUIView, LineView lucky)
        {
            _gameOverControls.UI.Enable();
            _gameOverControls.UI.GameSetting.performed += _ => settingsUIView.GameSettingToggle();
            _gameOverControls.UI.DialogueInput.performed += _ => lucky.OnContinueClicked();
        }

        // CH2
        public void CH2KeyBinding(CH2KeyBinder keyBinder)
        {
            _gameOverControls.UI.Enable();
            _gameOverControls.UI.GameSetting.performed += _ => keyBinder.SetSettingUI();
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