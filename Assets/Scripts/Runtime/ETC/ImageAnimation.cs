using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageAnimation : MonoBehaviour
{
    private Image _image;
    public Sprite[] Sprites;
    private int _animFrame = -1;
    [SerializeField]
    private float _animTime = 0.25f;

    public void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(NextSprite), _animTime, _animTime);
    }

    public void NextSprite()
    {
        if (Sprites.Length != 0)
        {
            _animFrame = ++_animFrame % Sprites.Length;
            _image.sprite = Sprites[_animFrame];
        }
    }
}