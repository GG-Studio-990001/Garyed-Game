using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class SpriteRotation
    {
        private readonly SpriteRenderer spriteRenderer;
        public bool canFlip { get; private set; }
        public bool canRotate { get; private set; }

        public SpriteRotation(SpriteRenderer spriteRenderer)
        {
            this.spriteRenderer = spriteRenderer;
        }

        public void SetCanFlip(bool canFlip)
        {
            this.canFlip = canFlip;
        }

        public void SetCanRotate(bool canRotate)
        {
            this.canRotate = canRotate;
        }

        public void FlipSprite(Vector2 direction)
        {
            if (canFlip && direction.y == 0)
            {
                spriteRenderer.flipX = (direction.x != 1);
            }
        }

        public int RotationZValue(Vector2 direction)
        {
            int zValue = 0;
            if (canRotate)
            {
                if (direction.x == 0)
                {
                    zValue = 90 * (int)direction.y * (spriteRenderer.flipX ? -1 : 1);
                }
            }
            return zValue;
        }
    }
}