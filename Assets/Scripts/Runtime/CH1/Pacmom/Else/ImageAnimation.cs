using UnityEngine;
using UnityEngine.UI;

namespace Runtime.CH1.Pacmom
{
    [RequireComponent(typeof(Image))]
    public class ImageAnimation : MonoBehaviour
    {
        private Image image;
        public Sprite[] sprites;
        private int animFrame = -1;
        private float animTime = 0.25f;

        public void Awake()
        {
            image = GetComponent<Image>();
        }

        private void Start()
        {
            InvokeRepeating("NextSprite", animTime, animTime);
        }

        public void NextSprite()
        {
            if (sprites.Length != 0)
            {
                animFrame = ++animFrame % sprites.Length;
                image.sprite = sprites[animFrame];
            }
        }
    }
}