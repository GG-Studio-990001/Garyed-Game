using Runtime.Interface;
using UnityEngine;

namespace Runtime.CH1.Main.Npc
{
    public class Npc : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private NpcInteraction _interaction;
        private IAnimation _animation;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _interaction = GetComponent<NpcInteraction>();
            _animation = new NpcAnimation(GetComponent<Animator>());

            _interaction.OnInteract += (direction) => _spriteRenderer.flipX = direction.x < 0;
        }
    }
}
