using Runtime.ETC;
using System.Linq;
using UnityEngine;

namespace Runtime.CH1.SubB
{
    public class ThreeMatchPuzzleController : MonoBehaviour
    {
        [SerializeField] private Jewelry[] jewelries;
        public bool IsClear { get; set; }
        
        private readonly Vector2[] _directions = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };
        
        private void Start()
        {
            foreach (var jewelry in jewelries)
            {
                jewelry.Controller = this;
            }
        }

        public void PuzzleReset()
        {
            foreach (var jewelry in jewelries)
            {
                jewelry.ResetPosition();
            }
        }
        
        public bool ValidateMovement(Jewelry jewelry, Vector2 direction)
        {
            if (IsClear)
            {
                return false;
            }
            
            Vector2 targetPosition = jewelry.transform.position + (Vector3) direction;
            
            foreach (var otherJewelry in jewelries)
            {
                if ((Vector2)otherJewelry.transform.position == targetPosition)
                {
                    return false;
                }
            }
            
            return true;
        }
        
        public void CheckMatching()
        {
            foreach (var jewelry in jewelries)
            {
                foreach (var direction in _directions)
                {
                    CheckingBf(jewelry.transform.position, jewelry.JewelryType, direction,3);
                }
            }
        }

        private bool CheckingBf(Vector2 position, JewelryType type, Vector2 direction, int length)
        {
            var jewelry = CheckAndReturnJewelry(position);
            
            if (jewelry == null)
            {
                return false;
            }
            if (jewelry.JewelryType != type)
            {
                return false;
            }
            if (length == 1)
            {
                return true;
            }
            
            if (CheckingBf(position + direction, type, direction,length - 1))
            {
                jewelry.DestroyJewelry();
                IsClear = true; // 1개라도 매칭되면 클리어
                return true;
            }
            
            return false;
        }

        private Jewelry CheckAndReturnJewelry(Vector2 position) => jewelries.FirstOrDefault(jewelry => (Vector2)jewelry.transform.position == position);
    }
}
