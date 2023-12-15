using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class MovementAndEyes : Movement
    {
        [SerializeField]
        private SpriteRenderer eyeSprite;
        [SerializeField]
        private Sprite[] EyeSprites;

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

        public void GetEyeSprites(Vector2 direction)
        {
            if (eyeSprite == null)
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