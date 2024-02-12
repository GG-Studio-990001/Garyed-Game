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

        public void SetEyeNormal(bool isNormalEye)
        {
            this.isNormalEye = isNormalEye;
        }

        private void Awake()
        {
            Set();
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
                case Vector2 v when v.Equals(Vector2.up): // 0 1
                    idx = 0;
                    break;
                case Vector2 v when v.Equals(Vector2.down): // 0 -1
                    idx = 1;
                    break;
                case Vector2 v when v.Equals(Vector2.right): // 1 0
                    idx = 2;
                    break;
                case Vector2 v when v.Equals(Vector2.left): // -1 0
                    idx = 3;
                    break;
            }

            eyeSprite.sprite = EyeSprites[idx];
        }
    }
}