using UnityEngine;

namespace Runtime.Interface
{
    public interface IAnimation
    {
        public bool SetAnimation(string stateName, Vector2 direction = default);
    }
}