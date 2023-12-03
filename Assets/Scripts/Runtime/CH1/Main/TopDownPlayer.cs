using Runtime.CH1.Main.PlayerFunction;
using Runtime.ETC;
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
        
        private PlayerState _state = PlayerState.Idle;
        private Vector2 _movementInput;
        private TopDownMovement _movement;
        private TopDownAnimation _animation;
        private TopDownInteraction _interaction;

        private void Awake()
        {
            _movement = new TopDownMovement(moveSpeed, transform);
            _animation = new TopDownAnimation(GetComponent<Animator>(), animSpeed);
            _interaction = new TopDownInteraction(transform, LayerMask.GetMask(GlobalConst.Interaction));
        }
        
        private void Update()
        {
            _animation.SetMovementAnimation(_state, _movementInput);
        }

        private void FixedUpdate()
        {
            bool isMove = _movement.Move(_movementInput);
            _state = isMove ? PlayerState.Move : PlayerState.Idle;
        }

        private void OnMove(InputValue value) => _movementInput = value.Get<Vector2>();

        private void OnInteraction()
        {
            bool isInteract = _interaction.Interact(_movement.Direction);
            _state = isInteract ? PlayerState.Interact : PlayerState.Idle;
        }
    }
}