using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class SpriteAnimation
    {
        private readonly SpriteRenderer _spriteRenderer;

        public Sprite[] sprites;
        private int _animFrame = -1;
        public bool isLoop { get; private set; }

        public SpriteAnimation(SpriteRenderer spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
            SetLoop(true);
        }

        public void SetLoop(bool isLoop)
        {
            this.isLoop = isLoop;
        }

        public void NextSprite()
        {
            if (!isLoop && _animFrame == sprites.Length - 1)
                return;

            if (sprites.Length == 1 && _animFrame == sprites.Length - 1)
                return;

            if (sprites.Length != 0)
            {
                _animFrame = ++_animFrame % sprites.Length;
                _spriteRenderer.sprite = sprites[_animFrame];
            }
        }

        public void RestartAnim()
        {
            _animFrame = -1;

            NextSprite();
        }
    }
}
