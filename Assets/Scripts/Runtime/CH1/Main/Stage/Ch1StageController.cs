using Cinemachine;
using Runtime.InGameSystem;
using UnityEngine;

namespace Runtime.CH1.Main.Stage
{
    public class Ch1StageController : MonoBehaviour
    {
        [field:SerializeField] public CinemachineConfiner2D Confiner2D { get; private set; }
        public Ch1StageChanger StageChanger { get; private set; }
        
        [SerializeField] private Stage[] stages;
        private FadeController _fadeController;
        private Transform _playerTransform;

        [SerializeField] private Vector3[] _stageDefaultPosition;

        public Stage CurrentStage => StageChanger.CurrentStage;
        
        public void Init(FadeController fadeController, Transform playerTransform)
        {
            _fadeController = fadeController;
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

            Managers.Data.Stage = moveStageNumber;
            Managers.Data.SaveGame();
        }
        
        private void StageChangerInit()
        {
            StageChanger = new Ch1StageChanger(_playerTransform, stages, _fadeController, Confiner2D);
            
            StageChanger.OnStageStart += () => Managers.Data.InGameKeyBinder.PlayerInputDisable();
            StageChanger.OnStageEnd += () => Managers.Data.InGameKeyBinder.PlayerInputEnable();
        }
        
        private async void StageSettings()
        {
            foreach (var stage in stages)
            {
                stage.StageSettings(StageChanger);
            }



#if UNITY_EDITOR
            // 아래는 개발용
            // 유니티 에디터라면 하이어라키에서 활성화된 스테이지의 지정 위치에서 시작
            foreach (var stage in stages)
            {
                if (stage.IsActivate())
                {
                    await StageChanger.SetStage(stage.StageNumber, _stageDefaultPosition[stage.StageNumber - 1]);
                    return;
                }
            }
#else
            // 빌드라면 저장된 스테이지의 지정 위치에서 시작
            foreach (var stage in stages)
            {
                if (stage.IsActivate())
                {
                    stage.Disable();
                }
            }
            await StageChanger.SetStage(Managers.Data.Stage, _stageDefaultPosition[Managers.Data.Stage - 1]);
#endif
        }
    }
}
