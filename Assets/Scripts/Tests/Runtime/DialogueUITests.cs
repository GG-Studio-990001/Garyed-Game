using NUnit.Framework;
using Runtime.Common.Presentation;
using Runtime.Common.View;
using UnityEngine;
using TMPro;

namespace Tests.Runtime
{
    [TestFixture]
    public class DialogueUITests
    {
        private GameObject _gameObject;
        private DialogueUIView _view;
        private DialogueUIPresenter _presenter;
        
        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject("DialogueUI");
            
            _view = _gameObject.AddComponent<DialogueUIView>();
            _view.dialogueText = new GameObject("DialogueText").AddComponent<TMPro.TextMeshProUGUI>();
            
            _presenter = new DialogueUIPresenter(_view);
        }
        
        [Test]
        public void WhenDialogueStartsViewIsUpdated()
        {
            _presenter.StartDialogue("Hello World!");
            Assert.AreEqual("Hello World!", _view.GetDialogueText());
        }
    }
}