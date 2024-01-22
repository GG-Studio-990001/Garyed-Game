using Cinemachine;
using Runtime.CH1.Main.Player;
using Runtime.Common.View;
using Runtime.Data.Original;
using Runtime.InGameSystem;
using Runtime.Interface;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.CH1.Main.Controller
{
    public class Ch1MainSystemController : MonoBehaviour
    {
        [Header("System")]
        [SerializeField] private SoundSystem soundSystem;
        [SerializeField] private SettingsUIView settingsUIView;
        [FormerlySerializedAs("stageController")] [SerializeField] private Ch1StageController ch1StageController;
        [SerializeField] private FadeController fadeController;
        
        [Header("Player")]
        [SerializeField] private TopDownPlayer player;
        
        [Header("Camera")]
        [SerializeField] private CinemachineConfiner2D cinemachineConfiner2D;
        
        [Header("Cursor")]
        [SerializeField] private Texture2D cursorTexture;
        
        private IProvider<ControlsData> ControlsDataProvider => DataProviderManager.Instance.ControlsDataProvider;
        private GameOverControls GameOverControls => ControlsDataProvider.Get().GameOverControls;
        
        private void Awake()
        {
            // MainSystem 시작 될 때
            GameInit();
            KeyBinding();
            
            // test
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
        
        private void GameInit()
        {
            fadeController.FadeDuration = 1;
            
            ch1StageController.Init(player.gameObject, cinemachineConfiner2D, fadeController, this);
            
            SetMusic("Start");
        }
        
        // 바인딩 해제 생각 (지금은 씬 이동 시 초기화)
        private void KeyBinding()
        {
            // player
            GameOverControls.Player.Enable();
            GameOverControls.Player.Move.performed += player.OnMove;
            GameOverControls.Player.Move.started += player.OnMove;
            GameOverControls.Player.Move.canceled += player.OnMove;
            GameOverControls.Player.Interaction.performed += _ => player.OnInteraction();
            
            // ui
            GameOverControls.UI.Enable();
            GameOverControls.UI.GameSetting.performed += _ => settingsUIView.GameSettingToggle();
        }
        
        private void SetMusic(string soundName)
        {
            soundSystem.StopMusic();
            soundSystem.PlayMusic(soundName);
        }
        
        public void PlayerInputLimit(bool isLimit)
        {
            if (isLimit)
            {
                GameOverControls.Player.Disable();
                GameOverControls.UI.Disable();
            }
            else
            {
                GameOverControls.Player.Enable();
                GameOverControls.UI.Enable();
            }
        }
    }
}