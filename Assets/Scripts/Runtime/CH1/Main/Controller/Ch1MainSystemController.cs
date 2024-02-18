using Cinemachine;
using Runtime.CH1.Main.Dialogue;
using Runtime.CH1.Main.Player;
using Runtime.CH1.Main.Stage;
using Runtime.Common.View;
using Runtime.Data.Original;
using Runtime.InGameSystem;
using Runtime.Input;
using Runtime.Interface;
using UnityEngine;

namespace Runtime.CH1.Main.Controller
{
    public class Ch1MainSystemController : MonoBehaviour
    {
        [Header("System")]
        [SerializeField] private SoundSystem soundSystem;
        [SerializeField] private SettingsUIView settingsUIView;
        [SerializeField] private Ch1StageController ch1StageController;
        [SerializeField] private Ch1DialogueController ch1DialogueController;
        [SerializeField] private TimelineController timelineController;
        [SerializeField] private FadeController fadeController;
        
        [Header("Player")]
        [SerializeField] private TopDownPlayer player;
        
        private IProvider<ControlsData> ControlsDataProvider => DataProviderManager.Instance.ControlsDataProvider;
        private GameOverControls GameOverControls => ControlsDataProvider.Get().GameOverControls;

        private InGameKeyBinder _inGameKeyBinder;
        
        private void Awake()
        {
            GameKeyBinding();
            GameInit();
        }
        
        private void GameKeyBinding()
        {
            _inGameKeyBinder = new InGameKeyBinder(GameOverControls);
            
            _inGameKeyBinder.PlayerKeyBinding(player);
            _inGameKeyBinder.UIKeyBinding(settingsUIView);
            
            ch1DialogueController.OnDialogueStart.AddListener(() => _inGameKeyBinder.PlayerInputDisable());
            ch1DialogueController.OnDialogueEnd.AddListener(() => _inGameKeyBinder.PlayerInputEnable());
            
            timelineController.PlayableDirector.played += (_) => _inGameKeyBinder.PlayerInputDisable();
            timelineController.PlayableDirector.stopped += (_) => _inGameKeyBinder.PlayerInputEnable();
            
            settingsUIView.OnSettingsOpen += () => _inGameKeyBinder.PlayerInputDisable();
            settingsUIView.OnSettingsClose += () => _inGameKeyBinder.PlayerInputEnable();
        }
        
        private void GameInit()
        {
            ch1StageController.Init(fadeController, _inGameKeyBinder, player.transform);
        }
    }
}