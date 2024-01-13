using UnityEngine;

namespace Runtime.Interface
{
    public interface IStage
    {
        public GameObject StageObject { get; set; }
        public int StageNumber { get; set; }
        public IStageController StageController { get; set; }
        public void SetSetting();
        public void Enable();
        public void Disable();
        public bool IsActivate();
    }
}