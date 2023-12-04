using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private Sprite[] sprites;
        private float animTime = 0.25f;
        private int animFrame = -1;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            InvokeRepeating("NextSprite", animTime, animTime);
        }

        private void NextSprite()
        {
            if (spriteRenderer.enabled)
            {
                spriteRenderer.sprite = sprites[++animFrame % sprites.Length];
            }
        }

        public void RestartAnim()
        {
            animFrame = -1;

            NextSprite();
        }
    }
}
