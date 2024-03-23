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
        private float _animTime = 0.25f;

        public void Awake()
        {
            spriteAnim = new SpriteAnimation(GetComponent<SpriteRenderer>());
        }

        private void Start()
        {
            InvokeRepeating("SpriteAnimation", _animTime, _animTime);
        }

        private void SpriteAnimation()
        {
            spriteAnim.NextSprite();
        }

        public virtual void GetNormalSprite()
        {
            spriteAnim.Sprites = new Sprite[normalSprites.Length];

            for (int i = 0; i < spriteAnim.Sprites.Length; i++)
            {
                spriteAnim.Sprites[i] = normalSprites[i];
            }
            spriteAnim.RestartAnim();
        }

        public virtual void GetVacuumModeSprite()
        {
            spriteAnim.Sprites = new Sprite[vacuumModeSprites.Length];

            for (int i = 0; i < spriteAnim.Sprites.Length; i++)
            {
                spriteAnim.Sprites[i] = vacuumModeSprites[i];
            }
            spriteAnim.RestartAnim();
        }
    }
}