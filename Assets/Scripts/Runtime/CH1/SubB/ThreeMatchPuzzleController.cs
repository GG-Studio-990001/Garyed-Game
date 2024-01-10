using UnityEngine;

namespace Runtime.CH1.SubB
{
    public class ThreeMatchPuzzleController : MonoBehaviour
    {
        [field:SerializeField] public Jewelry[] Jewelries { get; set; }
        public bool IsClear =>  _logic.IsClear;
        
        private ThreeMatchPuzzleLogic _logic;
        
        private void Start()
        {
            _logic = new ThreeMatchPuzzleLogic(Jewelries);
            
            foreach (var jewelry in Jewelries)
            {
                jewelry.Controller = this;
            }
        }

        public void PuzzleReset()
        {
            foreach (var jewelry in Jewelries)
            {
                jewelry.ResetPosition();
            }
        }
        
        public bool ValidateMovement(Jewelry jewelry, Vector2 direction) => _logic.ValidateMovement(jewelry, direction);
        public void CheckMatching() => _logic.CheckMatching();
    }
}
