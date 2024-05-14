using Runtime.CH1.Main.Interface;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace Runtime.CH1.SubB.SLG
{
    public class SlguiObject : MonoBehaviour, IInteractive
    {
        public UnityEvent onInteract;
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
            onInteract?.Invoke();

            SLGActionComponent SLGAction = FindObjectOfType<SLGActionComponent>();
            if (SLGAction != null)
            {
                if (Managers.Data.Scene < 4)
                {
                    // 임시로 막아두기
                    _dialogueRunner.StartDialogue("SLG_Block");
                }
                else
                {
                    // TODO: 다이얼로그 끝나면 효과음과 함께 UI 띄우기
                    _dialogueRunner.StartDialogue("SLG_Start");
                    SLGAction.OnSLGInit();
                }
            }

            return true;
        }
    
    }
}