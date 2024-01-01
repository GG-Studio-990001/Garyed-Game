using Runtime.CH1.Main.PlayerFunction;
using Runtime.ETC;
using Runtime.Interface;
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
        private IMovement _movement;
        private IAnimation _animation;
        private TopDownInteraction _interaction;
        private GameOverControls _gameOverControls;

        private void Start()
        {
            _movement = new TopDownMovement(moveSpeed, transform);
            _animation = new TopDownAnimation(GetComponent<Animator>(), animSpeed);
            _interaction = new TopDownInteraction(transform, LayerMask.GetMask(GlobalConst.Interaction));
            _gameOverControls = Ch1GameSystem.Instance.GameOverControls;
            
            _gameOverControls.Player.Move.performed += OnMove;
            _gameOverControls.Player.Move.started += OnMove;
            _gameOverControls.Player.Move.canceled += OnMove;
            _gameOverControls.Player.Interaction.performed += ctx => OnInteraction();
        }
        
        private void Update()
        {
            _animation.SetAnimation(_state.ToString(), _movementInput);
        }

        private void FixedUpdate()
        {
            bool isMove = _movement.Move(_movementInput);
            _state = isMove ? PlayerState.Move : PlayerState.Idle;
        }

        private void OnMove(InputAction.CallbackContext context) => _movementInput = context.ReadValue<Vector2>();

        private void OnInteraction()
        {
            bool isInteract = _interaction.Interact(_movement.Direction);
            _state = isInteract ? PlayerState.Interact : PlayerState.Idle;
        }
    }
}