using Runtime.CH1.Main.Interface;
using Runtime.ETC;
using System;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Main
{
    public class NpcInteraction : MonoBehaviour, IInteractive
    {
        private Npc _npc;
        private Vector2 _previousDirection;
        public Action<Vector2> OnInteract { get; set; }
        [SerializeField] private DialogueRunner _dialogueRunner;

        private void Awake()
        {
            _npc = GetComponent<Npc>();

            if (_dialogueRunner == null)
            {
                Debug.LogError("DialogueRunner is not found.");
            }
        }

        public bool Interact(Vector2 direction)
        {
            OnInteract?.Invoke(direction);
            NpcDirection(direction);

            _dialogueRunner.StartDialogue(gameObject.name);
            return true;
        }

        private void NpcDirection(Vector2 direction)
        {
            _previousDirection = _npc.Anim.GetDirection();

            Vector2 newDirection = new (direction.x * -1, direction.y * -1);
            _npc.Anim.SetAnimation(GlobalConst.IdleStr, newDirection);
        }

        public void ResetNpcDirection()
        {
            _npc.Anim.SetAnimation(GlobalConst.IdleStr, _previousDirection);
        }
    }
}