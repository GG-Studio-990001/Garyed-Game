using Cinemachine;
using Runtime.CH1.Main.Dialogue;
using Runtime.CH1.Main.Player;
using Runtime.CH1.Main.Stage;
using Runtime.CH1.Pacmom;
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
        [SerializeField] private SettingsUIView _settingsUIView;
        [SerializeField] private Ch1StageController _ch1StageController;
        [SerializeField] private Ch1DialogueController _ch1DialogueController;
        [SerializeField] private TimelineController _timelineController;
        [SerializeField] private FadeController _fadeController;
        
        [Header("Player")]
        [SerializeField] private TopDownPlayer _player;

        [Header("Else")] // 맵에서 초기화해줘야 하는 것 / 클래스 따로 빼기
        [SerializeField] private NpcPosition _npcPosition;
        [SerializeField] private LuckyPack _luckyPack;
        [SerializeField] private BridgeController _bridge;


        private void Start()
        {
            GameKeyBinding();
            GameInit();
            LoadGame();
            SetMap();
            StartBGM();
        }

        private void LoadGame()
        {
            Managers.Data.LoadGame();
        }

        private void SetMap()
        {
            _npcPosition.LoadNpcPosition();
            _luckyPack.ActiveLuckyPack();
            _bridge.CheckBridge();
        }

        // 저장된 데이터를 토대로 맵 이동
        private void StartBGM()
        {
            Managers.Sound.Play(Sound.BGM, "Ch1Main");
        }

        // 인게임에 사용되는 키 이벤트 바인딩
        private void GameKeyBinding()
        {
            Managers.Data.InGameKeyBinder.GameControlReset();
            
            Managers.Data.InGameKeyBinder.CH1PlayerKeyBinding(_player);
            Managers.Data.InGameKeyBinder.CH1UIKeyBinding(_settingsUIView);

            _ch1DialogueController.OnDialogueStart.AddListener(() => Managers.Data.InGameKeyBinder.PlayerInputDisable());
            _ch1DialogueController.OnDialogueEnd.AddListener(() => Managers.Data.InGameKeyBinder.PlayerInputEnable());

            _timelineController.PlayableDirector.played += (_) => Managers.Data.InGameKeyBinder.PlayerInputDisable();
            _timelineController.PlayableDirector.stopped += (_) => Managers.Data.InGameKeyBinder.PlayerInputEnable();

            _settingsUIView.OnSettingsOpen += () => Managers.Data.InGameKeyBinder.PlayerInputDisable();
            _settingsUIView.OnSettingsClose += () => Managers.Data.InGameKeyBinder.PlayerInputEnable();
        }
        
        // 각 컨트롤러 초기화
        private void GameInit()
        {
            _ch1StageController.Init(_fadeController, _player.transform);
        }
    }
}