using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class SpriteRotation
    {
        private readonly SpriteRenderer spriteRenderer;
        public bool CanFlip { get; private set; }
        public bool CanRotate { get; private set; }

        public SpriteRotation(SpriteRenderer spriteRenderer)
        {
            this.spriteRenderer = spriteRenderer;
        }

        public void SetCanFlip(bool canFlip)
        {
            CanFlip = canFlip;
        }

        public void SetCanRotate(bool canRotate)
        {
            CanRotate = canRotate;
        }

        public void FlipSprite(Vector2 direction)
        {
            if (CanFlip && direction.y == 0)
            {
                spriteRenderer.flipX = (direction.x != 1);
            }
        }

        public int RotationZValue(Vector2 direction)
        {
            int zValue = 0;
            if (CanRotate)
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