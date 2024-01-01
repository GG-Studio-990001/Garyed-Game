using Runtime.ETC;
using Runtime.Interface;
using UnityEngine;

namespace Runtime.CH1.Main.PlayerFunction
{
    public class TopDownAnimation : IAnimation
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

        public void SetAnimation(string stateName, Vector2 direction = default)
        {
            switch (stateName)
            {
                case nameof(PlayerState.Idle):
                    _animator.SetBool(Moving, false);
                    break;
                case nameof(PlayerState.Move):
                    _animator.SetBool(Moving, true);
                    _animator.SetFloat(Horizontal1, direction.x);
                    _animator.SetFloat(Vertical1, direction.y);
                    break;
                case nameof(PlayerState.Interact):
                    // TODO 애니메이션 추가
                    break;
                default:
                    //Debug.LogError("Invalid PlayerState");
                    break;  
            }
        }
    }
}