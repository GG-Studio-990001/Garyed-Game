using Cinemachine;
using Runtime.CH1.Main.Dialogue;
using Runtime.CH1.Main.Player;
using Runtime.CH1.Main.Stage;
using Runtime.Common.View;
using Runtime.Data.Original;
using Runtime.Event;
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
        
        private void Awake()
        {
            GameKeyBinding();
            GameInit();
            SetGame();
        }

        // 인게임에 사용되는 키 이벤트 바인딩
        private void GameKeyBinding()
        {
            Managers.Data.InGameKeyBinder.CH1PlayerKeyBinding(player);
            Managers.Data.InGameKeyBinder.CH1UIKeyBinding(settingsUIView);
            
            ch1DialogueController.OnDialogueStart.AddListener(() => Managers.Data.InGameKeyBinder.PlayerInputDisable());
            ch1DialogueController.OnDialogueEnd.AddListener(() => Managers.Data.InGameKeyBinder.PlayerInputEnable());
            
            timelineController.PlayableDirector.played += (_) => Managers.Data.InGameKeyBinder.PlayerInputDisable();
            timelineController.PlayableDirector.stopped += (_) => Managers.Data.InGameKeyBinder.PlayerInputEnable();
            
            settingsUIView.OnSettingsOpen += () => Managers.Data.InGameKeyBinder.PlayerInputDisable();
            settingsUIView.OnSettingsClose += () => Managers.Data.InGameKeyBinder.PlayerInputEnable();
        }
        
        // 각 컨트롤러 초기화
        private void GameInit()
        {
            ch1StageController.Init(fadeController, player.transform);
        }
        
        // 저장된 데이터를 토대로 맵 이동
        private void SetGame()
        {
            Managers.Sound.Play(Sound.BGM, "Ch1Main");
            
            // 개발자 모드
            #if UNITY_EDITOR
            
            #else // 빌드된다면 데이터로 읽기
            //ch1StageController.SetStage(Managers.Data.Stage, new Vector2(0, 0));
            #endif
        }
    }
}