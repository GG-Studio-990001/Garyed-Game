using UnityEngine;

namespace Runtime.Interface
{
    public interface IInteraction
    {
        public bool Interact(Vector2 direction = default);
    }
}