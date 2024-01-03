using Runtime.CH1.Main.PlayerFunction;
using Runtime.Data.Original;
using Runtime.ETC;
using Runtime.InGameSystem;
using Runtime.Interface;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace Runtime.CH1.Main.Player
{
    [RequireComponent(typeof(Animator))]
    public class TopDownPlayer : MonoBehaviour
    {
        // TODO 이거 데이터로 빼야함
        [SerializeField] private float moveSpeed = 5.0f;
        [SerializeField] private float animSpeed = 0.5f;
        
        private IMovement _movement;
        private IAnimation _animation;
        private IInteraction _interaction;
        
        private PlayerState _state = PlayerState.Idle;
        private Vector2 _movementInput;
        private IProvider<ControlsData> ControlsDataProvider => DataProviderManager.Instance.ControlsDataProvider;

        private void Start()
        {
            _movement = new TopDownMovement(moveSpeed, transform);
            _animation = new TopDownAnimation(GetComponent<Animator>(), animSpeed);
            _interaction = new TopDownInteraction(transform, LayerMask.GetMask(GlobalConst.Interaction));
            
            PlayerKeyBinding();
        }
        
        private void PlayerKeyBinding()
        {
            GameOverControls gameControls = ControlsDataProvider.Get().GameOverControls;
            
            gameControls.Player.Enable();
            gameControls.Player.Move.performed += OnMove;
            gameControls.Player.Move.started += OnMove;
            gameControls.Player.Move.canceled += OnMove;
            gameControls.Player.Interaction.performed += ctx => OnInteraction();
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