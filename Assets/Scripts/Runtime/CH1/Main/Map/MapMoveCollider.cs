using UnityEngine;

namespace Runtime.CH1.Main.Map
{
    public class MapMoveCollider : MonoBehaviour
    {
        [SerializeField] private int stageNumber;
        [SerializeField] private Vector2 movePosition; // TODO 이동할 위치를 설정할 수 있도록 변경
        public Ch1GameController Ch1GameController { get; set; }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (Ch1GameController == null)
                    Debug.Log("???");
                // Stage 번호 이동으로 변경 (뒤 앞 등등의 이동)
                Ch1GameController?.NextStage(stageNumber, movePosition);
            }
        }
    }
}
