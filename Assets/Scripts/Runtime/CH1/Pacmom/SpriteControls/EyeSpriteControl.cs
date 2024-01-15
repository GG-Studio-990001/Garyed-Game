using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class EyeSpriteControl : SpriteControl
    {
        public override void GetNormalSprite()
        {
            spriteAnim.SetLoop(false);
            base.GetNormalSprite();
        }

        public override void GetVacuumModeSprite()
        {
            spriteAnim.SetLoop(true);
            base.GetVacuumModeSprite();
        }
    }
}