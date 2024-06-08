using Runtime.Interface;
using UnityEngine;

namespace Runtime.CH1.Main.Npc
{
    public class NpcBody : MonoBehaviour
    {
        // private SpriteRenderer _spriteRenderer;
        // private NpcInteraction _interaction;
        public INpcAnim Anim { get; private set; }

        private void Awake()
        {
            // _spriteRenderer = GetComponent<SpriteRenderer>();
            // _interaction = GetComponent<NpcInteraction>();
            Anim = new NpcAnimation(GetComponent<Animator>());
        }
    }
}
