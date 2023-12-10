using Runtime.CH1.Pacmom;
using System.Collections;
using UnityEngine;

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

    public void Normal()
    {
        spriteAnim.sprites = new Sprite[normalSprites.Length];
        for (int i = 0; i < spriteAnim.sprites.Length; i++)
        {
            spriteAnim.sprites[i] = normalSprites[i];
        }
        spriteAnim.RestartAnim();
    }

    public void Vacuum()
    {
        spriteAnim.sprites = new Sprite[1];
        spriteAnim.sprites[0] = vacuumSprite;
    }

    public void VaccumBlink()
    {
        spriteAnim.sprites = new Sprite[2];
        spriteAnim.sprites[0] = vacuumSprite;
        spriteAnim.sprites[1] = normalSprite;
        spriteAnim.RestartAnim();
    }

    public void Die()
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
