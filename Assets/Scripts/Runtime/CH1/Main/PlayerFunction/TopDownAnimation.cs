using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Main.PlayerFunction
{
    public class TopDownAnimation
    {
        private readonly Animator _animator;
        private readonly float _animationSpeed;
        private static readonly int Moving = Animator.StringToHash(IsMoving);
        private static readonly int Horizontal1 = Animator.StringToHash(Horizontal);
        private static readonly int Vertical1 = Animator.StringToHash(Vertical);

        private const string IsMoving = "IsMoving";
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        
        public TopDownAnimation(Animator animator, float animationSpeed = 1.0f)
        {
            _animator = animator;
            _animator.speed = _animationSpeed = animationSpeed;
        }
        
        public void SetMovementAnimation(PlayerState state, Vector2 movementInput)
        {
            switch (state)
            {
                case PlayerState.Idle:
                    _animator.SetBool(Moving, false);
                    break;
                case PlayerState.Move:
                    _animator.SetBool(Moving, true);
                    _animator.SetFloat(Horizontal1, movementInput.x);
                    _animator.SetFloat(Vertical1, movementInput.y);
                    break;
                case PlayerState.Interact:
                    // TODO 애니메이션 추가
                    break;
                default:
                    //Debug.LogError("Invalid PlayerState");
                    break;
            }
            // // TODO 리터럴값 제거, 애니메이션 확장되는대로
            // if (movementInput == Vector2.zero)
            // {
            //     _animator.SetBool(Moving, false);
            //     return;
            // }
            //
            // _animator.SetBool(Moving, true);
            //
            // _animator.SetFloat(Horizontal1, movementInput.x);
            // _animator.SetFloat(Vertical1, movementInput.y);
        }
    }
}