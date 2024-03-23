using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class SpriteAnimation
    {
        private readonly SpriteRenderer _spriteRenderer;

        public Sprite[] Sprites;
        private int _animFrame = -1;
        public bool IsLoop { get; private set; }

        public SpriteAnimation(SpriteRenderer spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
            SetLoop(true);
        }

        public void SetLoop(bool isLoop)
        {
            IsLoop = isLoop;
        }

        public void NextSprite()
        {
            if (!IsLoop && _animFrame == Sprites.Length - 1)
                return;

            if (Sprites.Length == 1 && _animFrame == Sprites.Length - 1)
                return;

            if (Sprites.Length != 0)
            {
                _animFrame = ++_animFrame % Sprites.Length;
                _spriteRenderer.sprite = Sprites[_animFrame];
            }
        }

        public void RestartAnim()
        {
            _animFrame = -1;

            NextSprite();
        }
    }
}
