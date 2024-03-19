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
using Sound = Runtime.ETC.Sound;

namespace Runtime.CH1.Main.Controller
{
    public class Ch1MainSystemController : MonoBehaviour
    {
        [Header("System")]
        [SerializeField] private SettingsUIView settingsUIView;
        [SerializeField] private Ch1StageController ch1StageController;
        [SerializeField] private Ch1DialogueController ch1DialogueController;
        [SerializeField] private TimelineController timelineController;
        [SerializeField] private FadeController fadeController;
        
        [Header("Player")]
        [SerializeField] private TopDownPlayer player;

        private InGameKeyBinder _inGameKeyBinder;
        
        private void Awake()
        {
            GameKeyBinding();
            GameInit();
            SetGame();
        }

        // 인게임에 사용되는 키 이벤트 바인딩
        private void GameKeyBinding()
        {
            _inGameKeyBinder = new InGameKeyBinder(Managers.Data.GameOverControls);
            
            _inGameKeyBinder.PlayerKeyBinding(player);
            _inGameKeyBinder.UIKeyBinding(settingsUIView);
            
            ch1DialogueController.OnDialogueStart.AddListener(() => _inGameKeyBinder.PlayerInputDisable());
            ch1DialogueController.OnDialogueEnd.AddListener(() => _inGameKeyBinder.PlayerInputEnable());
            
            timelineController.PlayableDirector.played += (_) => _inGameKeyBinder.PlayerInputDisable();
            timelineController.PlayableDirector.stopped += (_) => _inGameKeyBinder.PlayerInputEnable();
            
            settingsUIView.OnSettingsOpen += () => _inGameKeyBinder.PlayerInputDisable();
            settingsUIView.OnSettingsClose += () => _inGameKeyBinder.PlayerInputEnable();
        }
        
        // 각 컨트롤러 초기화
        private void GameInit()
        {
            ch1StageController.Init(fadeController, _inGameKeyBinder, player.transform);
        }
        
        // 저장된 데이터를 토대로 맵 이동
        private void SetGame()
        {
            Managers.Sound.Play(Sound.BGM, "Ch1Main");
            ch1StageController.SetStage(Managers.Data.Stage, new Vector2(0, 0));
        }
    }
}