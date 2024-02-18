using Cinemachine;
using Runtime.CH1.Main.Controller;
using Runtime.InGameSystem;
using Runtime.Input;
using Runtime.Interface;
using UnityEngine;

namespace Runtime.CH1.Main.Stage
{
    public class Ch1StageController : MonoBehaviour, IStageController
    {
        public IStageChanger StageChanger { get; set; }
        [field:SerializeField] public CinemachineConfiner2D Confiner2D { get; private set; }
        
        private Stage[] _stages;
        private Ch1MainSystemController _mainSystemController;
        private FadeController _fadeController;
        private InGameKeyBinder _inGameKeyBinder;
        private Transform _playerTransform;
        
        public void Init(FadeController fadeController, InGameKeyBinder inGameKeyBinder, Transform playerTransform)
        {
            _fadeController = fadeController;
            _inGameKeyBinder = inGameKeyBinder;
            _playerTransform = playerTransform;
            
            _stages = GetComponentsInChildren<Stage>();
            
            StageChangerInit();
            
            foreach (var stage in _stages)
            {
                stage.StageController = this;
            }
            
            // 개발용: 인스펙터에서 스테이지를 바꿀 수 있도록 하기 위해
            foreach (var stage in _stages)
            {
                if (stage.IsActivate())
                {
                    StageChanger.SwitchStage(stage.StageNumber, new Vector2(0, 0));
                }
            }

            if (_fadeController is not null)
                _fadeController.StartFadeIn();
        }
        
        private void StageChangerInit()
        {
            StageChanger = new Ch1StageChanger(_playerTransform, _stages, _fadeController, Confiner2D);
            
            StageChanger.OnStageStart += () => _inGameKeyBinder.PlayerInputDisable();
            StageChanger.OnStageEnd += () => _inGameKeyBinder.PlayerInputEnable();
        }
    }
}
