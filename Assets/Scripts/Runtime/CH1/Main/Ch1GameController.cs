using UnityEngine;

namespace Runtime.CH1.Main
{
    public class Ch1GameController : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private TopDownPlayer topDownPlayer; // Inspector에서 Player를 넣어줘야함 or Find로 찾아서 넣어줘야함
        [Header("System")]
        [SerializeField] private GameObject gameSettingUI;
        [SerializeField] private SoundSystem soundSystem;

        public GameOverControls GameOverControls { get; private set; }

        private void Awake()
        {
            InitGame();
        }
        
        private void InitGame()
        {
            GameOverControls = new GameOverControls();
            
            SetAllUIActiveFalse();
            SetMusic("Start");
            SetKeyBinding();
            
            // DI
            topDownPlayer.Ch1GameSystem = this;
        }
        
        private void SetAllUIActiveFalse()
        {
            gameSettingUI.SetActive(false);
        }
        
        private void SetMusic(string soundName)
        {
            soundSystem.StopMusic();
            soundSystem.PlayMusic(soundName);
        }
        
        private void SetKeyBinding()
        {
            GameOverControls.Enable();
            GameOverControls.UI.GameSetting.performed += ctx => OnGameSetting();
        }
        
        private void OnGameSetting()
        {
            gameSettingUI.SetActive(gameSettingUI.activeSelf == false);
        }
        
        #region Unity Event

        public void OnDialogueStart()
        {
            GameOverControls.Player.Disable();
        }
        
        public void OnDialogueEnd()
        {
            GameOverControls.Player.Enable();
        }
        
        public void OnGameSettingStart()
        {
            GameOverControls.Player.Disable();
        }
        
        public void OnGameSettingEnd()
        {
            GameOverControls.Player.Enable();
        }

        #endregion
    }
}