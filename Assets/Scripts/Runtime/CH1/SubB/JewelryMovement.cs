using DG.Tweening;
using Runtime.Interface;
using UnityEngine;

namespace Runtime.CH1.SubB
{
    public class JewelryMovement : IMovement
    {
        public Vector2 Direction => _previousMovementInput;
        
        private readonly Transform _transform;
        private readonly Transform _spriteTransform;
        private readonly float _moveSpeed;
        private bool _isMoving = false;
        private Vector2 _previousMovementInput = Vector2.zero;
        
        public JewelryMovement(Transform transform, Transform spriteTransform, float moveSpeed)
        {
            _transform = transform;
            _spriteTransform = spriteTransform;
            _moveSpeed = moveSpeed;
        }
        
        public bool Move(Vector2 movementInput)
        {
            if (_isMoving)
            {
                return false;
            }
            
            _previousMovementInput = movementInput;
            
            Vector3 targetPosition = _transform.position + (Vector3)movementInput;

            _spriteTransform.DOShakePosition(_moveSpeed, new Vector3(0.01f, 0.01f, 0.01f), 30, 90f, false, false);
            _transform.DOMove(targetPosition, _moveSpeed)
                .SetEase(Ease.Linear)
                .OnComplete(() => _isMoving = false)
                .OnStart(() => _isMoving = true);
            
            return true;
        }
    }
}