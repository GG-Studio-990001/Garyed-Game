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
        
        [SerializeField] private Stage[] stages;
        private FadeController _fadeController;
        private InGameKeyBinder _inGameKeyBinder;
        private Transform _playerTransform;
        
        public Stage CurrentStage => StageChanger.CurrentStage;
        
        public void Init(FadeController fadeController, InGameKeyBinder inGameKeyBinder, Transform playerTransform)
        {
            _fadeController = fadeController;
            _inGameKeyBinder = inGameKeyBinder;
            _playerTransform = playerTransform;
            
            StageChangerInit();
            StageSettings();
        }
        
        public void SetStage(int moveStageNumber, Vector2 spawnPosition)
        {
            foreach (var stage in stages)
            {
                stage.Disable();
            }
            
            stages[moveStageNumber].Enable();
        }
        
        private void StageChangerInit()
        {
            StageChanger = new Ch1StageChanger(_playerTransform, stages, _fadeController, Confiner2D);
            
            StageChanger.OnStageStart += () => _inGameKeyBinder.PlayerInputDisable();
            StageChanger.OnStageEnd += () => _inGameKeyBinder.PlayerInputEnable();
        }
        
        private async void StageSettings()
        {
            foreach (var stage in stages)
            {
                stage.StageSettings(StageChanger);
            }
            
            // 아래는 개발용
            // 인스펙터에서 선택한 스테이지에서 시작 가능하게
            foreach (var stage in stages)
            {
                if (stage.IsActivate())
                {
                    await StageChanger.SetStage(stage.StageNumber, new Vector2(0, 0));
                }
            }
        }
    }
}
