using Runtime.CH1.Main.Interface;
using System;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Main
{
    public class NpcInteraction : MonoBehaviour, IInteractive
    {
        public Action<Vector2> OnInteract { get; set; }
        [SerializeField] private DialogueRunner dialogueRunner;

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

        public bool Interact(Vector2 direction)
        {
            OnInteract?.Invoke(direction);
            dialogueRunner.StartDialogue(gameObject.name);
            return true;
        }
    }
}