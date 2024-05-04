using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.CH1.Pacmom
{
    public class Rapley : MonoBehaviour
    {
        public MovementWithFlipAndRotate Movement { get; set; }

        private void Awake()
        {
            Movement = GetComponent<MovementWithFlipAndRotate>();
        }

        private void Start()
        {
            SetSpriteRotation();
            Movement.ResetState();
        }

        private void SetSpriteRotation()
        {
            Movement.SpriteRotation.SetCanRotate(false);
        }

        private void FixedUpdate()
        {
            Movement.Move();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 inputDirection = context.ReadValue<Vector2>();

            if (inputDirection != Vector2.zero)
            {
                Movement.SetNextDirection(inputDirection);
            }
        }
    }
}