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

            if (direction == Vector2.up)
            {
                eyeSprite.sprite = EyeSprites[0];
            }
            else if (direction == Vector2.down)
            {
                eyeSprite.sprite = EyeSprites[1];
            }
            else if (direction == Vector2.right)
            {
                eyeSprite.sprite = EyeSprites[2];
            }
            else if (direction == Vector2.left)
            {
                eyeSprite.sprite = EyeSprites[3];
            }
        }
    }
}