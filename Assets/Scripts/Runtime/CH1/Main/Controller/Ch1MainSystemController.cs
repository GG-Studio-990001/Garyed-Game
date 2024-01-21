using Cinemachine;
using Runtime.CH1.Main.Player;
using Runtime.Common.View;
using Runtime.Data.Original;
using Runtime.InGameSystem;
using Runtime.Interface;
using UnityEngine;

namespace Runtime.CH1.Main.Controller
{
    public class Ch1MainSystemController : MonoBehaviour
    {
        [Header("System")]
        [SerializeField] private SoundSystem soundSystem;
        [SerializeField] private SettingsUIView settingsUIView;
        [SerializeField] private StageController stageController;
        [SerializeField] private FadeController fadeController;
        
        [Header("Player")]
        [SerializeField] private TopDownPlayer player;
        
        [Header("Camera")]
        [SerializeField] private CinemachineConfiner2D cinemachineConfiner2D;
        
        [Header("Cursor")]
        [SerializeField] private Texture2D cursorTexture;
        
        private IProvider<ControlsData> ControlsDataProvider => DataProviderManager.Instance.ControlsDataProvider;
        
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
            stageController.Init(player.gameObject, cinemachineConfiner2D, fadeController);
            
            SetMusic("Start");
        }
        
        // 바인딩 해제 생각 (지금은 씬 이동 시 초기화)
        private void KeyBinding()
        {
            GameOverControls gameControls = ControlsDataProvider.Get().GameOverControls;
            
            // player
            gameControls.Player.Enable();
            gameControls.Player.Move.performed += player.OnMove;
            gameControls.Player.Move.started += player.OnMove;
            gameControls.Player.Move.canceled += player.OnMove;
            gameControls.Player.Interaction.performed += _ => player.OnInteraction();
            
            // ui
            gameControls.UI.Enable();
            gameControls.UI.GameSetting.performed += _ => settingsUIView.GameSettingToggle();
        }
        
        private void SetMusic(string soundName)
        {
            soundSystem.StopMusic();
            soundSystem.PlayMusic(soundName);
        }
    }
}