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
                // 다이얼로그러너를 하나 더 추가했으므로 임시 조치
                _dialogueRunner = GameObject.Find("DialogueRunner").GetComponent<DialogueRunner>(); // FindObjectOfType<DialogueRunner>();
                if (_dialogueRunner == null)
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
