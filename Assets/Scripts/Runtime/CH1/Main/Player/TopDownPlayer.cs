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
        
        public Ch1GameController Ch1GameSystem { get; set; }
        
        private IMovement _movement;
        private IAnimation _animation;
        private IInteraction _interaction;
        
        private PlayerState _state = PlayerState.Idle;
        private Vector2 _movementInput;

        private void Start()
        {
            _movement = new TopDownMovement(moveSpeed, transform);
            _animation = new TopDownAnimation(GetComponent<Animator>(), animSpeed);
            _interaction = new TopDownInteraction(transform, LayerMask.GetMask(GlobalConst.Interaction));
            
            PlayerKeyBinding();
        }
        
        private void PlayerKeyBinding()
        {
            if (Ch1GameSystem == null)
                return;
            
            Ch1GameSystem.GameOverControls.Player.Enable();
            
            Ch1GameSystem.GameOverControls.Player.Move.performed += OnMove;
            Ch1GameSystem.GameOverControls.Player.Move.started += OnMove;
            Ch1GameSystem.GameOverControls.Player.Move.canceled += OnMove;
            Ch1GameSystem.GameOverControls.Player.Interaction.performed += ctx => OnInteraction();
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
        
        private void OnInteraction()
        {
            bool isInteract = _interaction.Interact(_movement.Direction);
            _state = isInteract ? PlayerState.Interact : PlayerState.Idle;
        }
        
        private void OnMove(InputAction.CallbackContext context) => _movementInput = context.ReadValue<Vector2>();
    }
}