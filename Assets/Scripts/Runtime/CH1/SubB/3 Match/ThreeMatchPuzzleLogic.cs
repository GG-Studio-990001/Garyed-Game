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
        
        public ThreeMatchPuzzleLogic(List<Jewelry> jewelries)
        {
            this._jewelries = jewelries;
        }

        public bool ValidateMovement(Jewelry jewelry, Vector2 direction)
        {
            Vector3Int currentCell = jewelry.Tilemap.WorldToCell(jewelry.transform.position);
            Vector3Int targetCell = currentCell + new Vector3Int((int)direction.x, (int)direction.y, 0);
            
            foreach (var otherJewelry in _jewelries)
            {
                Vector3Int otherCell = jewelry.Tilemap.WorldToCell(otherJewelry.transform.position);
                if (otherJewelry.JewelryType == JewelryType.Disappear)
                {
                    continue;
                }
                if (otherCell == targetCell)
                {
                    return false;
                }
                if (otherJewelry.IsMoving)
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
                if (jewelry.JewelryType == JewelryType.None || jewelry.JewelryType == JewelryType.Disappear)
                {
                    continue;
                }
                
                Vector3 currentCell = jewelry.Tilemap.WorldToCell(jewelry.transform.position);
                foreach (var direction in _directions)
                {
                    CheckingBf(currentCell, jewelry.JewelryType, direction, 3);
                }
            }
            
            foreach (var jewelry in _jewelriesToDestroy)
            {
                jewelry.DestroyJewelry();
            }
            
            _jewelriesToDestroy.Clear();
        }

        private bool CheckingBf(Vector3 position, JewelryType type, Vector2 direction, int length)
        {
            var jewelry = CheckAndReturnJewelry(position);
            
            if (jewelry == null || jewelry.JewelryType == JewelryType.Disappear || jewelry.JewelryType == JewelryType.None || jewelry.gameObject.activeSelf == false)
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

            Vector3 nextCell = position + new Vector3((int)direction.x, (int)direction.y, 0);
            if (CheckingBf(nextCell, type, direction,length - 1))
            {
                _jewelriesToDestroy.Add(jewelry);
                return true;
            }
            
            return false;
        }

        private Jewelry CheckAndReturnJewelry(Vector3 position) => _jewelries.FirstOrDefault(jewelry => jewelry.Tilemap.WorldToCell(jewelry.transform.position) == position);
    }
}