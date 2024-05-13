using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteControl : MonoBehaviour
    {
        [SerializeField]
        protected SpriteAnimation _spriteAnim;
        [SerializeField]
        protected Sprite[] _normalSprites;
        [SerializeField]
        protected Sprite[] _vacuumModeSprites;
        private readonly float _animTime = 0.25f;

        public void Awake()
        {
            _spriteAnim = new SpriteAnimation(GetComponent<SpriteRenderer>());
        }

        public void StartAnim()
        {
            InvokeRepeating(nameof(SpriteAnimation), _animTime, _animTime);
        }

        private void SpriteAnimation()
        {
            _spriteAnim.NextSprite();
        }

        public virtual void GetNormalSprite()
        {
            _spriteAnim.Sprites = new Sprite[_normalSprites.Length];

            for (int i = 0; i < _spriteAnim.Sprites.Length; i++)
            {
                _spriteAnim.Sprites[i] = _normalSprites[i];
            }
            _spriteAnim.RestartAnim();
        }

        public virtual void GetVacuumModeSprite()
        {
            _spriteAnim.Sprites = new Sprite[_vacuumModeSprites.Length];

            for (int i = 0; i < _spriteAnim.Sprites.Length; i++)
            {
                _spriteAnim.Sprites[i] = _vacuumModeSprites[i];
            }
            _spriteAnim.RestartAnim();
        }
    }
}