using UnityEngine;

namespace Runtime.CH1.Main.Dialogue
{
    public class NpcDialogueController : MonoBehaviour
    {
        [SerializeField] private NpcInteraction[] _npc = new NpcInteraction[4];
        // 0 달러 1 파머 2 알투몬 3 마마고

        public void NpcDialogueFin(int idx)
        {
            _npc[idx].ResetNpcDirection();
        }
    }
}