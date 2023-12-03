using Runtime.CH1.Main.Interface;
using Runtime.Common.Presentation;
using System;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Main
{
    public class NpcInteraction : MonoBehaviour, IInteractive
    {
        [SerializeField] private string talkToNode = "";
        [SerializeField] private DialogueRunner dialogueRunner;

        public bool Interact()
        {
            dialogueRunner?.StartDialogue(talkToNode);
            
            return true;
        }
    }
}