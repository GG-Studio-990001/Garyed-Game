using Runtime.Common.Presentation;
using TMPro;
using UnityEngine;

namespace Runtime.Common.View
{
    public class DialogueUIView : MonoBehaviour
    {
        [field:SerializeField] public TextMeshProUGUI dialogueText;
        
        private DialogueUIPresenter _presenter;

        private void Start()
        {
            _presenter = PresenterFactory.CreateDialogueUIPresenter(this);
        }
        
        public void StartDialogue(string text)
        {
            dialogueText.text = text;
        }
        
        public string GetDialogueText()
        {
            return dialogueText.text;
        }
        
        public DialogueUIPresenter GetPresenter()
        {
            return _presenter;
        }
    }
}