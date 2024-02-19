using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class SpriteAnimation
    {
        private readonly SpriteRenderer spriteRenderer;

        public Sprite[] sprites;
        private int animFrame = -1;
        public bool isLoop { get; private set; }

        public SpriteAnimation(SpriteRenderer spriteRenderer)
        {
            this.spriteRenderer = spriteRenderer;
            SetLoop(true);
        }

        public void SetLoop(bool isLoop)
        {
            this.isLoop = isLoop;
        }

        public void NextSprite()
        {
            if (!isLoop && animFrame == sprites.Length - 1)
                return;

            if (sprites.Length == 1 && animFrame == sprites.Length - 1)
                return;

            if (sprites.Length != 0)
            {
                animFrame = ++animFrame % sprites.Length;
                spriteRenderer.sprite = sprites[animFrame];
            }
        }

        public void RestartAnim()
        {
            animFrame = -1;

            NextSprite();
        }
    }
}
