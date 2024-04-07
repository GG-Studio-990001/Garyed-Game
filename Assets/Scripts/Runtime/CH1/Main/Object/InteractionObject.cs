using Runtime.CH1.Main.Interface;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace Runtime.CH1.Main.Object
{
    public class InteractionObject : MonoBehaviour, IInteractive
    {
        public UnityEvent onInteract;
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
            onInteract?.Invoke();
            dialogueRunner.StartDialogue(gameObject.name);
            return true;
        }
    }
}
