using Cinemachine;
using Runtime.CH1.Main.Map;
using UnityEngine;

namespace Runtime.CH1.Main.Controller
{
    public class StageController : MonoBehaviour
    {
        public Stage CurrentStage { get; set; }
        public CinemachineConfiner2D Confiner2D { get; private set; }
        private Stage[] _stages;
        private GameObject _player;
        
        public void Init(GameObject player, CinemachineConfiner2D confiner2D)
        {
            _player = player;
            Confiner2D = confiner2D;
            
            _stages = GetComponentsInChildren<Stage>();
            
            foreach (var stage in _stages)
            {
                stage.Init(this);
            }
            
            // 개발용: 인스펙터에서 스테이지를 바꿀 수 있도록 하기 위해
            foreach (var stage in _stages)
            {
                if (stage.IsActivate())
                {
                    CurrentStage = stage;
                    CurrentStage.SetMapSetting();
                }
            }
        }

        public void SwitchStage(int moveStageNumber, Vector2 spawnPosition)
        {
            if (CurrentStage != null)
                CurrentStage.StageDisable();
            
            CurrentStage = _stages[moveStageNumber - 1];
            
            _player.transform.position = spawnPosition;
            
            CurrentStage.StageEnable();
            CurrentStage.SetMapSetting();
        }
    }
}
