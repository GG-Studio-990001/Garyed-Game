using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class RapleySpriteChange : MonoBehaviour
    {
        [SerializeField]
        private SpriteAnimation spriteAnimation;
        [SerializeField]
        private Sprite[] normalSprites;
        [SerializeField]
        private Sprite[] frightenedSprites;

        public void GetFrightendSprite(bool isFrightend)
        {
            Sprite[] newSprites;

            if (isFrightend)
            {
                newSprites = frightenedSprites;
            }
            else
            {
                newSprites = normalSprites;
            }

            spriteAnimation.sprites = new Sprite[newSprites.Length];

            for (int i = 0; i < newSprites.Length; i++)
            {
                spriteAnimation.sprites[i] = newSprites[i];
            }
        }
    }
}
