using System;
using UnityEngine;

namespace Runtime.Interface
{
    public interface IStageChanger
    {
        public void SwitchStage(int moveStageNumber, Vector2 spawnPosition);
        public Action OnStageStart { get; set; }
        public Action OnStageEnd { get; set; }
    }
}