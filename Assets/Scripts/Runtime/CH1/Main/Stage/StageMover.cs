using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Main.Stage
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class StageMover : MonoBehaviour
    {
        [SerializeField] private int moveStageNumber;
        [SerializeField] private Vector2 spawnPosition;

        private Stage _currentStage;
        
        private void Awake()
        {
            _currentStage = GetComponentInParent<Stage>();
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GlobalConst.PlayerStr))
            {
                _currentStage.StageController.StageChanger.SwitchStage(moveStageNumber, spawnPosition);
            }
        }
    }
}
