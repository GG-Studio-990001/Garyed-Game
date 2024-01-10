using Runtime.CH1.Main.Controller;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Runtime.CH1.Main.Map
{
    public class Stage : MonoBehaviour
    {
        [Header("Stage Info")]
        [SerializeField] private GameObject stageObject;
        [field: SerializeField] public int StageNumber { get; set; }
        
        [Header("Stage Extension")]
        [SerializeField] private PolygonCollider2D confiner2D;
        public UnityEvent onStageEnable;
         
        public StageController StageController { get; private set; }

        public void Init(StageController stageController)
        {
            StageController = stageController;
        }

        public void SetMapSetting()
        {
            StageController.Confiner2D.m_BoundingShape2D = confiner2D;
        }
        
        public bool IsActivate()
        {
            return stageObject.activeSelf;
        }
        
        public void StageEnable()
        {
            stageObject.SetActive(true);
            onStageEnable?.Invoke();
        }
        
        public void StageDisable()
        {
            stageObject.SetActive(false);
        }
    }
}
