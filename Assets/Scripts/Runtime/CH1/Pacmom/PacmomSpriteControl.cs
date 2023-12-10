using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PacmomSpriteControl : MonoBehaviour
    {
        [SerializeField]
        private SpriteAnimation spriteAnim;

        [SerializeField]
        private Sprite vacuumSprite;
        [SerializeField]
        private Sprite normalSprite;
        [SerializeField]
        private Sprite[] normalSprites;
        [SerializeField]
        private Sprite[] dieSprites;
        
        private void Awake()
        {
            spriteAnim = new SpriteAnimation(GetComponent<SpriteRenderer>());
        }

        private void Start()
        {
            InvokeRepeating("SpriteAnimation", spriteAnim.animTime, spriteAnim.animTime);
        }

        private void SpriteAnimation()
        {
            spriteAnim.NextSprite();
        }

        public void GetNormalSprite()
        {
            spriteAnim.sprites = new Sprite[normalSprites.Length];

            for (int i = 0; i < spriteAnim.sprites.Length; i++)
            {
                spriteAnim.sprites[i] = normalSprites[i];
            }
        }

        public void GetVacuumSprite()
        {
            spriteAnim.sprites = new Sprite[1];
            spriteAnim.sprites[0] = vacuumSprite;
            spriteAnim.RestartAnim();
        }

        public void SetVaccumSpriteBlink()
        {
            spriteAnim.sprites = new Sprite[2];
            spriteAnim.sprites[0] = vacuumSprite;
            spriteAnim.sprites[1] = normalSprite;
            spriteAnim.RestartAnim();
        }

        public void GetDieSprite()
        {
            spriteAnim.sprites = new Sprite[dieSprites.Length];
            for (int i = 0; i < dieSprites.Length; i++)
            {
                spriteAnim.sprites[i] = dieSprites[i];
            }
            spriteAnim.isLoop = false;
            spriteAnim.RestartAnim();
        }
    }
}