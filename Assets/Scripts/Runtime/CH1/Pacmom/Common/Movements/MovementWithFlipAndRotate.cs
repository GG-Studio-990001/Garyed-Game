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

            if (SpriteRotation.CanRotate)
            {
                SetRotateZ(0);
            }
        }

        protected override void SetDirection(Vector2 direction)
        {
            base.SetDirection(direction);

            if (!CheckRoadBlocked(direction))
            {
                SpriteRotation.FlipSprite(direction);

                if (SpriteRotation.CanRotate)
                {
                    SetRotateZ(SpriteRotation.GetZValue(direction));
                }
            }
        }

        public void SetRotateZ(int zValue = 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, zValue);
        }
    }
}