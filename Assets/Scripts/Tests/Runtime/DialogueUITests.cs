using NUnit.Framework;
using Runtime.Common.Presentation;
using Runtime.Common.View;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
    [TestFixture]
    public class DialogueUITests
    {
        private GameObject _gameObject;
        private DialogueUIView _view;
        private DialogueUIPresenter _presenter;
        
        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _gameObject = new GameObject("DialogueUI");
            
            _view = _gameObject.AddComponent<DialogueUIView>();
            _view.dialogueText = new GameObject("DialogueText").AddComponent<TMPro.TextMeshProUGUI>();
            
            _presenter = new DialogueUIPresenter(_view);
            
            yield return new WaitForFixedUpdate();
        }
        
        [UnityTest]
        public IEnumerator WhenDialogueStartsViewIsUpdated()
        {
            _presenter.StartDialogue("Hello World!");
            Assert.AreEqual("Hello World!", _view.GetDialogueText());
            
            yield return new WaitForFixedUpdate();
        }
    }
}