using Runtime.Interface;
using UnityEngine;

namespace Runtime.CH1.Main
{
    public class TopDownMovement : IMovement
    {
        public Vector2 Direction => _previousMovementInput;
        
        private readonly Transform _transform;
        private readonly float _moveSpeed = 5.0f;
        private Vector2 _previousMovementInput = Vector2.zero;
        
        public TopDownMovement(float moveSpeed, Transform transform)
        {
            _moveSpeed = moveSpeed;
            _transform = transform;
        }
        
        public bool Move(Vector2 movementInput)
        {
            if (movementInput == Vector2.zero)
            {
                return false;
            }
            
            _previousMovementInput = movementInput;
            
            if (movementInput.magnitude > 1.0f)
            {
                movementInput = _previousMovementInput;
            }
            
            Vector2 movement = movementInput * (_moveSpeed * Time.deltaTime);
            
            _transform.Translate(movement);
            
            return true;
        }
    }
}