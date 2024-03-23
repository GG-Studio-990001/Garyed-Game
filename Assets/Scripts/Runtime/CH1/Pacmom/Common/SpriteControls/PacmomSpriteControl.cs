using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PacmomSpriteControl : SpriteControl
    {
        [SerializeField]
        private Sprite[] _dieSprites;

        public void GetVaccumBlinkSprite()
        {
            spriteAnim.Sprites = new Sprite[2];
            spriteAnim.Sprites[0] = vacuumModeSprites[0];
            spriteAnim.Sprites[1] = normalSprites[0];
            spriteAnim.RestartAnim();
        }

        public void GetDieSprite()
        {
            spriteAnim.Sprites = new Sprite[_dieSprites.Length];
            for (int i = 0; i < _dieSprites.Length; i++)
            {
                spriteAnim.Sprites[i] = _dieSprites[i];
            }
            spriteAnim.SetLoop(false);
            spriteAnim.RestartAnim();
        }
    }
}