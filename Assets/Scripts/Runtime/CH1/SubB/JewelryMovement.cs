using DG.Tweening;
using Runtime.Interface;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Runtime.CH1.SubB
{
    public class JewelryMovement : IMovement
    {
        public bool IsMoving => _isMoving;
        public Vector2 Direction => _previousMovementInput;
        
        private bool _isMoving;
        private readonly Transform _transform;
        private readonly float _moveSpeed;
        private readonly Tilemap _tilemap;
        private Vector2 _previousMovementInput = Vector2.zero;
        
        public JewelryMovement(Transform transform, float moveSpeed, Tilemap tilemap)
        {
            _transform = transform;
            _moveSpeed = moveSpeed;
            _tilemap = tilemap;
        }
        
        public bool Move(Vector2 movementInput)
        {
            if (_isMoving)
            {
                return false;
            }
            
            _previousMovementInput = movementInput;
            Vector3Int currentCell = _tilemap.WorldToCell(_transform.position);
            Vector3Int targetCell = currentCell + new Vector3Int((int)movementInput.x, (int)movementInput.y, 0);

            Vector3 targetPosition = _tilemap.GetCellCenterWorld(targetCell); //_transform.position + (Vector3)movementInput;
            
            _transform.DOShakePosition(_moveSpeed, new Vector3(0.01f, 0.01f, 0.01f), 30, 90f, false, false);
            _transform.DOMove(targetPosition, _moveSpeed)
                .SetEase(Ease.Linear)
                .OnComplete(() => _isMoving = false)
                .OnStart(() => _isMoving = true);
            
            return true;
        }
    }
}