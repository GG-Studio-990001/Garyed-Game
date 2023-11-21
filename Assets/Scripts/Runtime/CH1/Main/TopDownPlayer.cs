using Runtime.CH1.Main.PlayerFunction;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.CH1.Main
{
    [RequireComponent(typeof(Animator))]
    public class TopDownPlayer : MonoBehaviour
    {
        // TODO 이거 데이터로 빼야함
        [SerializeField] private float moveSpeed = 5.0f;
        [SerializeField] private float animSpeed = 0.5f;
        
        private Vector2 _movementInput;
        private TopDownMovement _movement;
        private TopDownAnimation _animation;
        private TopDownInteraction _interaction;
        
        private const string Interaction = "Object";

        private void Awake()
        {
            _movement = new TopDownMovement(moveSpeed, transform);
            
            _animation = new TopDownAnimation(GetComponent<Animator>(), animSpeed);
            
            _interaction = new TopDownInteraction(transform, LayerMask.GetMask(Interaction));
        }
        
        private void Update()
        {
            _animation.SetMovementAnimation(_movementInput);
        }

        private void FixedUpdate()
        {
            _movement.Move(_movementInput);
        }

        private void OnMove(InputValue value)
        {
            _movementInput = value.Get<Vector2>();
        }
        
        private void OnInteraction()
        {
            Debug.Log("Interaction"+_movement.Direction);
            _interaction.Interact(_movement.Direction);
        }
    }
}