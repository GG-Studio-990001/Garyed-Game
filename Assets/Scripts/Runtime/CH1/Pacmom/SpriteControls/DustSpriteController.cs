using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class DustSpriteController : SpriteController
    {
        [SerializeField]
        private GameObject eyes;

        public override void GetNormalSprite()
        {
            base.GetNormalSprite();

            eyes.SetActive(true);
        }

        public override void GetVacuumModeSprite()
        {
            base.GetVacuumModeSprite();

            eyes.SetActive(false);
        }
    }
}
