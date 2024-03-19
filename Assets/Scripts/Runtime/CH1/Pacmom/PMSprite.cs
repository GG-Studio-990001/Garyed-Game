using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PMSprite : MonoBehaviour
    {
        [SerializeField]
        private SpriteControl rapleySprite;
        [SerializeField]
        private PacmomSpriteControl pacmomSprite;
        [SerializeField]
        private SpriteControl[] dustBodySprites = new SpriteControl[GlobalConst.DustCnt];
        [SerializeField]
        private SpriteControl[] dustEyeSprites = new SpriteControl[GlobalConst.DustCnt];

        private void Start()
        {
            SetNormalSprites();
        }

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

        public void SetVacuumModeSprites()
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