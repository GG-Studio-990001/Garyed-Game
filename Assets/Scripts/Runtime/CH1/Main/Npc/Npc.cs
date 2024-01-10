using UnityEngine;

namespace Runtime.CH1.Main.Npc
{
    public class Npc : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private NpcInteraction _npcInteraction;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _npcInteraction = GetComponent<NpcInteraction>();
            
            _npcInteraction.OnInteract += (direction) => _spriteRenderer.flipX = direction.x < 0;
        }
    }
}
