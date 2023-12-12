using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class SpriteRotation
    {
        private readonly SpriteRenderer spriteRenderer;
        public bool canFlip = false;
        public bool canRotate = false;

        public SpriteRotation(SpriteRenderer spriteRenderer)
        {
            this.spriteRenderer = spriteRenderer;
        }

        public float RotationZValue(Vector2 direction)
        {
            float zValue = 0f;
            if (canRotate)
            {
                if (direction.x == 0)
                {
                    zValue = 90 * direction.y * (spriteRenderer.flipX ? -1 : 1);
                }
            }
            return zValue;
        }

        public void FlipSprite(Vector2 direction)
        {
            if (canFlip && direction.y == 0)
            {
                spriteRenderer.flipX = (direction.x == 1 ? false : true);
            }
        }
    }
}