using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PMSprite : MonoBehaviour
    {
        [SerializeField]
        private SpriteControl _rapleySprite;
        [SerializeField]
        private PacmomSpriteControl _pacmomSprite;
        [SerializeField]
        private SpriteControl[] _dustBodySprites = new SpriteControl[GlobalConst.DustCnt];
        [SerializeField]
        private SpriteControl[] _dustEyeSprites = new SpriteControl[GlobalConst.DustCnt];

        private void Start()
        {
            SetNormalSprites();
        }

        public void SetNormalSprites()
        {
            _rapleySprite.GetNormalSprite();
            _pacmomSprite.GetNormalSprite();

            for (int i = 0; i < GlobalConst.DustCnt; i++)
            {
                _dustBodySprites[i].GetNormalSprite();
                _dustEyeSprites[i].GetNormalSprite();
            }
        }

        public void SetVacuumModeSprites()
        {
            _pacmomSprite.GetVacuumModeSprite();
            _rapleySprite.GetVacuumModeSprite();

            for (int i = 0; i < GlobalConst.DustCnt; i++)
            {
                _dustBodySprites[i].GetVacuumModeSprite();
                _dustEyeSprites[i].GetVacuumModeSprite();
            }
        }

        public void SetPacmomDieSprite()
        {
            _pacmomSprite.GetDieSprite();
        }

        public void SetPacmomBlinkSprite()
        {
            _pacmomSprite.GetVaccumBlinkSprite();
        }
    }
}