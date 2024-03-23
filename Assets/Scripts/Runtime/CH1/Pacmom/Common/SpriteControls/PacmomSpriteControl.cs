using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PacmomSpriteControl : SpriteControl
    {
        [SerializeField]
        private Sprite[] _dieSprites;

        public void GetVaccumBlinkSprite()
        {
            _spriteAnim.Sprites = new Sprite[2];
            _spriteAnim.Sprites[0] = _vacuumModeSprites[0];
            _spriteAnim.Sprites[1] = _normalSprites[0];
            _spriteAnim.RestartAnim();
        }

        public void GetDieSprite()
        {
            _spriteAnim.Sprites = new Sprite[_dieSprites.Length];
            for (int i = 0; i < _dieSprites.Length; i++)
            {
                _spriteAnim.Sprites[i] = _dieSprites[i];
            }
            _spriteAnim.SetLoop(false);
            _spriteAnim.RestartAnim();
        }
    }
}