using UnityEngine;

namespace Runtime.CH1.Main.Map
{
    public class MapMoveCollider : MonoBehaviour
    {
        public Ch1GameController Ch1GameController { get; set; }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Stage 번호 이동으로 변경 (뒤 앞 등등의 이동)
                Ch1GameController?.NextStage();
            }
        }
    }
}
