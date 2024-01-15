using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PMSpriteController : MonoBehaviour
    {
        [SerializeField]
        private SpriteControl rapleySprite;
        [SerializeField]
        private PacmomSpriteControl pacmomSprite;
        [SerializeField]
        private SpriteControl[] dustBodySprites = new SpriteControl[GlobalConst.DustCnt];
        [SerializeField]
        private EyeSpriteControl[] dustEyeSprites = new EyeSpriteControl[GlobalConst.DustCnt];

        public void SetNormalSprites()
        {
            rapleySprite.GetNormalSprite();
            pacmomSprite.GetNormalSprite();

            for (int i = 0; i < GlobalConst.DustCnt; i++)
            {
                dustBodySprites[i].GetNormalSprite();
                dustEyeSprites[i].GetNormalSprite();
            }
        }

        public void SetVaccumModeSprites()
        {
            pacmomSprite.GetVacuumModeSprite();
            rapleySprite.GetVacuumModeSprite();

            for (int i = 0; i < GlobalConst.DustCnt; i++)
            {
                dustBodySprites[i].GetVacuumModeSprite();
                dustEyeSprites[i].GetVacuumModeSprite();
            }
        }

        public void SetPacmomDieSprite()
        {
            pacmomSprite.GetDieSprite();
        }

        public void SetPacmomBlinkSprite()
        {
            pacmomSprite.GetVaccumBlinkSprite();
        }
    }
}