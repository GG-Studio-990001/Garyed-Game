using Runtime.CH1.Main.Controller;
using UnityEngine;

namespace Runtime.CH1.Main.Map
{
    public class Stage : MonoBehaviour
    {
        [Header("Stage Info")]
        [SerializeField] private GameObject stageObject;
        [field: SerializeField] public int StageNumber { get; set; }
        
        [Header("Stage Extension")]
        [SerializeField] private PolygonCollider2D confiner2D;
        
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
        }
        
        public void StageDisable()
        {
            stageObject.SetActive(false);
        }
    }
}
