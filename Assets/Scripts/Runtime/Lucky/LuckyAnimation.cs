using Runtime.Interface;
using UnityEngine;

namespace Runtime.Luck
{
    public class LuckyAnimation
    {
        private readonly Animator _animator;
        
        private static readonly int Walking = Animator.StringToHash(IsWalking);
        private static readonly int Pointing = Animator.StringToHash(IsPointing);
        private static readonly int Showing = Animator.StringToHash(IsShowing);
        private static readonly int Clapping = Animator.StringToHash(IsClapping);

        private const string IsWalking = "IsWalking";
        private const string IsPointing = "IsPointing";
        private const string IsShowing = "IsShowing";
        private const string IsClapping = "IsClapping";
        // fish

        public LuckyAnimation(Animator animator)
        {
            _animator = animator;
        }

        public void ResetAnim()
        {
            _animator.SetBool(Walking, false);
            _animator.SetBool(Pointing, false);
            _animator.SetBool(Showing, false);
            _animator.SetBool(Clapping, false);
        }

        public void SetAnimation(string stateName, Vector2 direction = default)
        {
            ResetAnim();

            switch (stateName)
            {
                //case "Idle":
                //    break;
                case "Walking":
                    _animator.SetBool(Walking, true);
                    break;
                case "Pointing":
                    _animator.SetBool(Pointing, true);
                    break;
                case "Showing":
                    _animator.SetBool(Showing, true);
                    break;
                case "Clapping":
                    _animator.SetBool(Clapping, true);
                    break;
                default:
                    Debug.LogError("Invalid State");
                    break;
            }
        }
    }
}