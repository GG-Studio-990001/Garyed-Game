using Cinemachine;
using Runtime.CH1.Main.Controller;
using Runtime.InGameSystem;
using Runtime.Input;
using UnityEngine;

namespace Runtime.CH1.Main.Stage
{
    public class Ch1StageController : MonoBehaviour
    {
        [field:SerializeField] public CinemachineConfiner2D Confiner2D { get; private set; }
        public Ch1StageChanger StageChanger { get; private set; }
        
        private Stage[] _stages;
        private Ch1MainSystemController _mainSystemController;
        private FadeController _fadeController;
        private InGameKeyBinder _inGameKeyBinder;
        private Transform _playerTransform;
        
        public Stage CurrentStage => StageChanger.CurrentStage;
        
        public void Init(FadeController fadeController, InGameKeyBinder inGameKeyBinder, Transform playerTransform)
        {
            _fadeController = fadeController;
            _inGameKeyBinder = inGameKeyBinder;
            _playerTransform = playerTransform;
            
            _stages = GetComponentsInChildren<Stage>();
            
            StageChangerInit();
            StageSettings();
        }
        
        private void StageChangerInit()
        {
            StageChanger = new Ch1StageChanger(_playerTransform, _stages, _fadeController, Confiner2D);
            
            StageChanger.OnStageStart += () => _inGameKeyBinder.PlayerInputDisable();
            StageChanger.OnStageEnd += () => _inGameKeyBinder.PlayerInputEnable();
        }
        
        private void StageSettings()
        {
            foreach (var stage in _stages)
            {
                stage.StageSettings(StageChanger);
            }
            
            // 아래는 개발용
            // 인스펙터에서 선택한 스테이지에서 시작 가능하게
            foreach (var stage in _stages)
            {
                if (stage.IsActivate())
                {
                    StageChanger.SetStage(stage.StageNumber, new Vector2(0, 0));
                }
            }
        }
    }
}
