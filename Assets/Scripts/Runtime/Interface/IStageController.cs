using UnityEngine;

namespace Runtime.Interface
{
    public interface IStageController
    {
        public IStage CurrentStage { get; set; }
        public IStageChanger StageChanger { get; set; }
    }
}