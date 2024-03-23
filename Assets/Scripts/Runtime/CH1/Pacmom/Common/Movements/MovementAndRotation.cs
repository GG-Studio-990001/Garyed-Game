using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MovementAndRotation : Movement
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

            if (SpriteRotation.CanFlip)
                SpriteRotation.FlipSprite(Direction);
        }

        protected override void SetDirection(Vector2 direction)
        {
            base.SetDirection(direction);

            if (!CheckRoadBlocked(direction))
            {
                int zValue = SpriteRotation.RotationZValue(direction);
                transform.rotation = Quaternion.Euler(0, 0, zValue);

                SpriteRotation.FlipSprite(direction);
            }
        }
    }
}