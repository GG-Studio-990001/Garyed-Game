using UnityEngine;

namespace Runtime.CH1.Main.Interface
{
    public interface IInteractive
    {
        public bool Interact(Vector2 direction = default);
    }
}