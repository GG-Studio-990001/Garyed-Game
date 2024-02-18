using UnityEngine;

namespace Runtime.Interface
{
    public interface IStageController
    {
        public IStageChanger StageChanger { get; set; }
    }
}