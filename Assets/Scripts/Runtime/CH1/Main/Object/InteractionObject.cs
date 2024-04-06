using Runtime.CH1.Main.Interface;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Main.Object
{
    public class InteractionObject : MonoBehaviour, IInteractive
    {
        private DialogueRunner dialogueRunner;
    
        private void Awake()
        {
            if (dialogueRunner == null)
            {
                dialogueRunner = FindObjectOfType<DialogueRunner>();
                if (dialogueRunner == null)
                {
                    Debug.LogError("DialogueRunner is not found.");
                }
            }
        }

        public bool Interact(Vector2 direction = default)
        {
            dialogueRunner.StartDialogue(gameObject.name);
            return true;
        }
    }
}
