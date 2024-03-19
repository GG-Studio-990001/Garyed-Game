using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PacmomSpriteControl : SpriteControl
    {
        [SerializeField]
        private Sprite[] _dieSprites;

        public void GetVaccumBlinkSprite()
        {
            spriteAnim.sprites = new Sprite[2];
            spriteAnim.sprites[0] = vacuumModeSprites[0];
            spriteAnim.sprites[1] = normalSprites[0];
            spriteAnim.RestartAnim();
        }

        public void GetDieSprite()
        {
            spriteAnim.sprites = new Sprite[_dieSprites.Length];
            for (int i = 0; i < _dieSprites.Length; i++)
            {
                spriteAnim.sprites[i] = _dieSprites[i];
            }
            spriteAnim.SetLoop(false);
            spriteAnim.RestartAnim();
        }
    }
}