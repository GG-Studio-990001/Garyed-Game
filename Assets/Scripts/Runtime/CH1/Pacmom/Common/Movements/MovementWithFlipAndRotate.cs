using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MovementWithFlipAndRotate : Movement
    {
        public SpriteRotation SpriteRotation { get; private set; }

        private void Awake()
        {
            SpriteRotation = new SpriteRotation(GetComponent<SpriteRenderer>());

            SetWhenAwake();
        }

        public override void ResetState()
        {
            base.ResetState();

            SpriteRotation.FlipSprite(Direction);
        }

        protected override void SetDirection(Vector2 direction)
        {
            base.SetDirection(direction);

            if (!CheckRoadBlocked(direction))
            {
                SpriteRotation.FlipSprite(direction);

                if (SpriteRotation.CanRotate)
                {
                    int zValue = SpriteRotation.RotationZValue(direction);
                    transform.rotation = Quaternion.Euler(0, 0, zValue);
                }
            }
        }
    }
}