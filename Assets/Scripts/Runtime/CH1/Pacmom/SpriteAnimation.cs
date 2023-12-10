using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class SpriteAnimation
    {
        private readonly SpriteRenderer spriteRenderer;
        public Sprite[] sprites;
        public float animTime { get; private set; }
        private int animFrame = -1;
        public bool isLoop = true;

        public SpriteAnimation(SpriteRenderer spriteRenderer)
        {
            this.spriteRenderer = spriteRenderer;
            animTime = 0.25f;
        }

        public void NextSprite()
        {
            if (!isLoop && animFrame == sprites.Length - 1)
                return;

            if (sprites.Length != 0)
            {
                spriteRenderer.sprite = sprites[++animFrame % sprites.Length];
            }
        }

        public void RestartAnim()
        {
            animFrame = -1;

            NextSprite();
        }
    }
}
