using Runtime.Interface.Pacmom;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.CH1.Pacmom
{
    public class Rapley : MonoBehaviour, ICharacter
    {
        public MovementAndRotation Movement { get; set; }

        private void Awake()
        {
            Movement = GetComponent<MovementAndRotation>();
        }

        private void Start()
        {
            SetSpriteRotation();
            ResetState();
        }

        private void SetSpriteRotation()
        {
            Movement.SpriteRotation.SetCanRotate(false);
            Movement.SpriteRotation.SetCanFlip(true);
        }

        public void ResetState()
        {
            Movement.ResetState();
        }

        private void FixedUpdate()
        {
            Movement.Move();
        }

        private void OnMove(InputValue value)
        {
            Vector2 inputDirection = value.Get<Vector2>();

            if (inputDirection != Vector2.zero)
            {
                Movement.SetNextDirection(inputDirection);
            }
        }
    }
}