using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MovementAndRotation : Movement
    {
        public SpriteRotation spriteRotation { get; private set; }

        private void Awake()
        {
            spriteRotation = new SpriteRotation(GetComponent<SpriteRenderer>());

            SetWhenAwake();
        }

        public override void ResetState()
        {
            base.ResetState();

            if (spriteRotation.canFlip)
                spriteRotation.FlipSprite(direction);
        }

        protected override void SetDirection(Vector2 direction)
        {
            base.SetDirection(direction);

            if (!CheckRoadBlocked(direction))
            {
                int zValue = spriteRotation.RotationZValue(direction);
                transform.rotation = Quaternion.Euler(0, 0, zValue);

                spriteRotation.FlipSprite(direction);
            }
        }
    }
}