using UnityEngine;

namespace Runtime.Interface
{
    public interface IMovement
    {
        public Vector2 Direction { get; }
        public bool Move(Vector2 movementInput);
    }
}