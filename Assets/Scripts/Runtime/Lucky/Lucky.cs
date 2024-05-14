using UnityEngine;

namespace Runtime.Luck
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Lucky : MonoBehaviour
    {
        private SpriteRenderer _spriteRender;
        public LuckyAnimation Anim { get; private set; }

        private void Awake()
        {
            _spriteRender = GetComponent<SpriteRenderer>();
            Anim = new LuckyAnimation(GetComponent<Animator>());
        }

        public void SetFlipX(bool flip)
        {
            _spriteRender.flipX = flip;
        }    
    }
}