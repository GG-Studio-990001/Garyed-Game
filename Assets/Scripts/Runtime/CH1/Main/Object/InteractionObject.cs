using Runtime.CH1.Main.Interface;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace Runtime.CH1.Main.Object
{
    public class InteractionObject : MonoBehaviour, IInteractive
    {
        public UnityEvent OnInteract;
        private DialogueRunner _dialogueRunner;
    
        private void Awake()
        {
            if (_dialogueRunner == null)
            {
                // TODO: Find 지양
                if (!GameObject.Find("DialogueRunner").TryGetComponent<DialogueRunner>(out _dialogueRunner))
                {
                    Debug.LogError("DialogueRunner is not found.");
                }
            }
        }

        public bool Interact(Vector2 direction = default)
        {
            OnInteract?.Invoke();
            _dialogueRunner.StartDialogue(gameObject.name);
            return true;
        }
    }
}
