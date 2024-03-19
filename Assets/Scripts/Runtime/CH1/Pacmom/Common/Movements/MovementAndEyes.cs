using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class MovementAndEyes : Movement
    {
        [SerializeField]
        private SpriteRenderer eyeSprite;
        [SerializeField]
        private Sprite[] EyeSprites;
        [SerializeField]
        private bool isNormalEye { get; set; }

        private void Awake()
        {
            SetWhenAwake();
        }

        public void SetEyeNormal(bool isNormalEye)
        {
            this.isNormalEye = isNormalEye;
        }

        protected override void SetDirection(Vector2 direction)
        {
            base.SetDirection(direction);

            if (!CheckRoadBlocked(direction))
            {
                GetEyeSprites(direction);
            }
        }

        public void GetEyeSpriteByPosition()
        {
            GetEyeSprites(transform.localPosition.x < 0 ? Vector2.right : Vector2.left);
        }

        public void GetEyeSprites(Vector2 direction)
        {
            if (eyeSprite is null || !isNormalEye)
                return;

            // int idx = (direction.x == 0 ? 0 : 2) + (direction.x + direction.y == -1 ? 1 : 0);

            int idx = 0;

            switch (direction)
            {
                case Vector2 v when v.Equals(Vector2.up):
                    idx = 0;
                    break;
                case Vector2 v when v.Equals(Vector2.down):
                    idx = 1;
                    break;
                case Vector2 v when v.Equals(Vector2.right):
                    idx = 2;
                    break;
                case Vector2 v when v.Equals(Vector2.left):
                    idx = 3;
                    break;
            }

            eyeSprite.sprite = EyeSprites[idx];
        }
    }
}