using UnityEngine;

namespace Runtime.Interface
{
    public interface INpcAnim : IAnimation
    {
        public Vector2 GetDirection();
    }
}