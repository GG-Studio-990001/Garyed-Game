using UnityEngine;

namespace Runtime.Interface
{
    public interface IStageController
    {
        public IStage CurrentStage { get; set; }
        public void SwitchStage(int moveStageNumber, Vector2 spawnPosition);
    }
}