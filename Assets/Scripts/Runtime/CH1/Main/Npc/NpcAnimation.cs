using Runtime.ETC;
using Runtime.Interface;
using UnityEngine;

namespace Runtime.CH1.Main.Npc
{
    public class NpcAnimation : INpcAnim
    {
        private readonly Animator _animator;
        private static readonly int Moving = Animator.StringToHash(IsMoving);
        private static readonly int Horizontal1 = Animator.StringToHash(Horizontal);
        private static readonly int Vertical1 = Animator.StringToHash(Vertical);

        private const string IsMoving = "IsMoving";
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";

        public NpcAnimation(Animator animator)
        {
            _animator = animator;
            _animator.speed = 0.5f;
        }

        public bool SetAnimation(string stateName, Vector2 direction = default)
        {
            switch (stateName)
            {
                case GlobalConst.IdleStr:
                    _animator.SetBool(Moving, false);
                    _animator.SetFloat(Horizontal1, direction.x);
                    _animator.SetFloat(Vertical1, direction.y);
                    break;
                case GlobalConst.MoveStr:
                    _animator.SetBool(Moving, true);
                    _animator.SetFloat(Horizontal1, direction.x);
                    _animator.SetFloat(Vertical1, direction.y);
                    break;
                default:
                    Debug.LogError("Invalid PlayerState");
                    return false;
            }

            _animator.GetFloat(Horizontal1);

            return true;
            // 왜 bool일까?
        }

        public Vector2 GetDirection()
        {
            return new(_animator.GetFloat(Horizontal1), _animator.GetFloat(Vertical1));
        }
    }
}