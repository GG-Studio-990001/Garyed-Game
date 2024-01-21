using Cinemachine;
using Runtime.InGameSystem;
using Runtime.Interface;
using System.Collections;
using UnityEngine;

namespace Runtime.CH1.Main.Controller
{
    public class StageController : MonoBehaviour, IStageController
    {
        public IStage CurrentStage { get; set; }
        public CinemachineConfiner2D Confiner2D { get; private set; }
        public FadeController FadeController { get; private set; }
        private IStage[] _stages;
        private GameObject _player;
        
        public void Init(GameObject player, CinemachineConfiner2D confiner2D, FadeController fadeController)
        {
            _player = player;
            Confiner2D = confiner2D;
            FadeController = fadeController;
            
            _stages = GetComponentsInChildren<IStage>();
            
            foreach (var stage in _stages)
            {
                stage.StageController = this;
            }
            
            // 개발용: 인스펙터에서 스테이지를 바꿀 수 있도록 하기 위해
            foreach (var stage in _stages)
            {
                if (stage.IsActivate())
                {
                    CurrentStage = stage;
                    CurrentStage.SetSetting();
                }
            }
            
            if (FadeController is not null)
                FadeController.FadeIn(1);
        }

        public void SwitchStage(int moveStageNumber, Vector2 spawnPosition)
        {
            if (FadeController is null)
            {
                NotCoroutineSwitchStage(moveStageNumber, spawnPosition);
            }
            else
            {
                StartCoroutine(CoroutineSwitchStage(moveStageNumber, spawnPosition));
            }
        }
        
        // stageChanger 클래스로 변경
        private IEnumerator CoroutineSwitchStage(int moveStageNumber, Vector2 spawnPosition)
        {
            FadeController.FadeOut(1);
            
            yield return new WaitForSeconds(1);
            
            if (CurrentStage != null)
                CurrentStage.Disable();
            
            CurrentStage = _stages[moveStageNumber - 1];
            
            _player.transform.position = spawnPosition;
            
            CurrentStage.Enable();
            CurrentStage.SetSetting();
            
            yield return new WaitForSeconds(0.5f);
            
            FadeController.FadeIn(1);
        }
        
        private void NotCoroutineSwitchStage(int moveStageNumber, Vector2 spawnPosition)
        {
            CurrentStage?.Disable();

            CurrentStage = _stages[moveStageNumber - 1];
            
            _player.transform.position = spawnPosition;
            
            CurrentStage.Enable();
            CurrentStage.SetSetting();
        }
    }
}
