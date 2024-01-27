using Runtime.ETC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.CH1.SubB
{
    public class ThreeMatchPuzzleLogic
    {
        private readonly List<Jewelry> _jewelries;
        private List<Jewelry> _jewelriesToDestroy = new List<Jewelry>();
        
        private readonly Vector2[] _directions = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };
        
        public Action IsClear { get; set; }
        
        public ThreeMatchPuzzleLogic(List<Jewelry> jewelries)
        {
            this._jewelries = jewelries;
        }

        public bool ValidateMovement(Jewelry jewelry, Vector2 direction)
        {
            Vector2 targetPosition = jewelry.transform.position + (Vector3) direction;
            
            foreach (var otherJewelry in _jewelries)
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
            foreach (var jewelry in _jewelries)
            {
                foreach (var direction in _directions)
                {
                    CheckingBf(jewelry.transform.position, jewelry.JewelryType, direction,3);
                }
            }
            
            foreach (var jewelry in _jewelriesToDestroy)
            {
                jewelry.DestroyJewelry();
            }
            
            _jewelriesToDestroy.Clear();
            
            ClearCheck();
        }

        private bool CheckingBf(Vector2 position, JewelryType type, Vector2 direction, int length)
        {
            var jewelry = CheckAndReturnJewelry(position);
            
            if (jewelry == null)
            {
                return false;
            }
            if (jewelry.JewelryType != type || jewelry.JewelryType == JewelryType.None)
            {
                return false;
            }
            if (length == 1)
            {
                return true;
            }
            
            if (CheckingBf(position + direction, type, direction,length - 1))
            {
                _jewelriesToDestroy.Add(jewelry);
                return true;
            }
            
            return false;
        }

        private Jewelry CheckAndReturnJewelry(Vector2 position) => _jewelries.FirstOrDefault(jewelry => (Vector2)jewelry.transform.position == position);

        private void ClearCheck()
        {
            if (_jewelries.Any(jewelry => jewelry.JewelryType != JewelryType.None))
            {
                return;
            }
            
            IsClear?.Invoke();
        }
    }
}