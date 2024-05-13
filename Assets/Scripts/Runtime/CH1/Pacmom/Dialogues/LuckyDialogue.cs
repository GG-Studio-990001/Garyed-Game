using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Pacmom
{
    public class LuckyDialogue : DialogueViewBase
    {
        private DialogueRunner _runner;

        private void Awake()
        {
            _runner = GetComponent<DialogueRunner>();
        }
    }
}