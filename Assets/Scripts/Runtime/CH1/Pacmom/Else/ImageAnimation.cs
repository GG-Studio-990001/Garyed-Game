using UnityEngine;
using UnityEngine.UI;

namespace Runtime.CH1.Pacmom
{
    [RequireComponent(typeof(Image))]
    public class ImageAnimation : MonoBehaviour
    {
        private Image _image;
        public Sprite[] sprites;
        private int _animFrame = -1;
        private float _animTime = 0.25f;

        public void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            InvokeRepeating("NextSprite", _animTime, _animTime);
        }

        public void NextSprite()
        {
            if (sprites.Length != 0)
            {
                _animFrame = ++_animFrame % sprites.Length;
                _image.sprite = sprites[_animFrame];
            }
        }
    }
}