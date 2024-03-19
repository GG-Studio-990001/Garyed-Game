using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PacmomSpriteControl : SpriteControl
    {
        [SerializeField]
        private Sprite[] dieSprites;

        public void GetVaccumBlinkSprite()
        {
            spriteAnim.sprites = new Sprite[2];
            spriteAnim.sprites[0] = vacuumModeSprites[0];
            spriteAnim.sprites[1] = normalSprites[0];
            spriteAnim.RestartAnim();
        }

        public void GetDieSprite()
        {
            spriteAnim.sprites = new Sprite[dieSprites.Length];
            for (int i = 0; i < dieSprites.Length; i++)
            {
                spriteAnim.sprites[i] = dieSprites[i];
            }
            spriteAnim.SetLoop(false);
            spriteAnim.RestartAnim();
        }
    }
}