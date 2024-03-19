using Runtime.CH1.Main.Interface;
using System;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Main
{
    public class NpcInteraction : MonoBehaviour, IInteractive
    {
        [SerializeField] private string talkToNode = "";
        [SerializeField] private DialogueRunner dialogueRunner;

        public Action<Vector2> OnInteract { get; set; }

        private void Awake()
        {
            if (dialogueRunner == null)
            {
                dialogueRunner = FindObjectOfType<DialogueRunner>();
            }
        }

        public bool Interact(Vector2 direction)
        {
            dialogueRunner.StartDialogue(talkToNode);
            
            OnInteract?.Invoke(direction);
            
            return true;
        }
    }
}