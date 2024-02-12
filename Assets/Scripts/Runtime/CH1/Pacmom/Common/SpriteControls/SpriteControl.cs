using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteControl : MonoBehaviour
    {
        [SerializeField]
        protected SpriteAnimation spriteAnim;
        [SerializeField]
        protected Sprite[] normalSprites;
        [SerializeField]
        protected Sprite[] vacuumModeSprites;
        private float animTime = 0.25f;

        public void Awake()
        {
            spriteAnim = new SpriteAnimation(GetComponent<SpriteRenderer>());
        }

        private void Start()
        {
            InvokeRepeating("SpriteAnimation", animTime, animTime);
        }

        private void SpriteAnimation()
        {
            spriteAnim.NextSprite();
        }

        public virtual void GetNormalSprite()
        {
            spriteAnim.sprites = new Sprite[normalSprites.Length];

            for (int i = 0; i < spriteAnim.sprites.Length; i++)
            {
                spriteAnim.sprites[i] = normalSprites[i];
            }
            spriteAnim.RestartAnim();
        }

        public virtual void GetVacuumModeSprite()
        {
            spriteAnim.sprites = new Sprite[vacuumModeSprites.Length];

            for (int i = 0; i < spriteAnim.sprites.Length; i++)
            {
                spriteAnim.sprites[i] = vacuumModeSprites[i];
            }
            spriteAnim.RestartAnim();
        }
    }
}