using UnityEngine;

namespace Runtime.Interface
{
    public interface IAnimation
    {
        public void SetAnimation(string stateName, Vector2 direction = default);
    }
}