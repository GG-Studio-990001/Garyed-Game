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
        
        private Ch1StageChanger _stageChanger;
        private Stage[] _stages;
        private Ch1MainSystemController _mainSystemController;
        private FadeController _fadeController;
        private InGameKeyBinder _inGameKeyBinder;
        private Transform _playerTransform;
        
        public Stage CurrentStage => _stageChanger.CurrentStage;
        
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
            _stageChanger = new Ch1StageChanger(_playerTransform, _stages, _fadeController, Confiner2D);
            
            _stageChanger.OnStageStart += () => _inGameKeyBinder.PlayerInputDisable();
            _stageChanger.OnStageEnd += () => _inGameKeyBinder.PlayerInputEnable();
        }
        
        private void StageSettings()
        {
            foreach (var stage in _stages)
            {
                stage.StageSettings(_stageChanger);
            }
            
            // 아래는 개발용
            // 인스펙터에서 선택한 스테이지에서 시작 가능하게
            foreach (var stage in _stages)
            {
                if (stage.IsActivate())
                {
                    _stageChanger.SwitchStage(stage.StageNumber, new Vector2(0, 0));
                }
            }

            if (_fadeController is not null)
                _fadeController.StartFadeIn();
        }
    }
}
