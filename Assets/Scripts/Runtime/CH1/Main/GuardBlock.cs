using Runtime.ETC;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Main
{
    public class GuardBlock : MonoBehaviour
    {
        [SerializeField] private DialogueRunner _dialogueRunner;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GlobalConst.PlayerStr))
            {
                _dialogueRunner.StartDialogue("Guard");
            }
        }
    }
}