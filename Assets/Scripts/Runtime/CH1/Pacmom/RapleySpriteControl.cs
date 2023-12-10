using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class RapleySpriteControl : MonoBehaviour
    {
        [SerializeField]
        private SpriteAnimation spriteAnim;
        [SerializeField]
        private Sprite[] normalSprites;
        [SerializeField]
        private Sprite[] frightenedSprites;

        public void GetNormalSprite(bool isNormal)
        {
            Sprite[] newSprites;

            if (isNormal)
            {
                newSprites = normalSprites;
            }
            else
            {
                newSprites = frightenedSprites;
            }

            spriteAnim.sprites = new Sprite[newSprites.Length];

            for (int i = 0; i < newSprites.Length; i++)
            {
                spriteAnim.sprites[i] = newSprites[i];
            }
        }
    }
}
