using UnityEngine;

namespace Runtime.Lucky
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class LuckyBody : MonoBehaviour
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