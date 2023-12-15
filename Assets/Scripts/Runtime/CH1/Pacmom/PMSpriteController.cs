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
        private DustSpriteControl[] dustSprites = new DustSpriteControl[GlobalConst.DustCnt];

        public void SetNormalSprites()
        {
            rapleySprite.GetNormalSprite();
            pacmomSprite.GetNormalSprite();

            for (int i = 0; i < dustSprites.Length; i++)
            {
                dustSprites[i].GetNormalSprite();
            }
        }

        public void SetVaccumModeSprites()
        {
            pacmomSprite.GetVacuumModeSprite();
            rapleySprite.GetVacuumModeSprite();

            for (int i = 0; i < dustSprites.Length; i++)
            {
                dustSprites[i].GetVacuumModeSprite();
            }
        }

        public void SetPacmomDieSprirte()
        {
            pacmomSprite.GetDieSprite();
        }

        public void SetPacmomBlinkSprirte()
        {
            pacmomSprite.GetVaccumBlinkSprite();
        }
    }
}