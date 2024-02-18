using Runtime.CH1.Main.Controller;
using Runtime.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.CH1.Main.Stage
{
    public class Stage : MonoBehaviour
    {
        [field:SerializeField] public GameObject StageObject { get; set; }
        [field: SerializeField] public int StageNumber { get; set; }
        public IStageController StageController { get; set; }

        [Header("Stage Extension")]
        [SerializeField] private PolygonCollider2D confiner2D;
        public UnityEvent onStageEnable;
        
        public PolygonCollider2D GetStageCollider()
        {
            return confiner2D;
        }
        
        public bool IsActivate()
        {
            return StageObject.activeSelf;
        }
        
        public void Enable()
        {
            StageObject.SetActive(true);
            onStageEnable?.Invoke();
        }
        
        public void Disable()
        {
            StageObject.SetActive(false);
        }
    }
}
