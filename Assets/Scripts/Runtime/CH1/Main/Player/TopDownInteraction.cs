using Runtime.CH1.Main.Interface;
using Runtime.Interface;
using UnityEngine;

namespace Runtime.CH1.Main.Player
{
    public class TopDownInteraction : IInteraction
    {
        private readonly Transform _transform;
        private readonly int _npcLayerMask;
        private readonly int _objectLayerMask;
        private readonly float _interactionDistance;
        
        public TopDownInteraction(Transform transform, int npcLayerMask, int objectLayerMask, float interactionDistance = 0.7f)
        {
            _transform = transform;
            _npcLayerMask = npcLayerMask;
            _objectLayerMask = objectLayerMask;
            _interactionDistance = interactionDistance;
        }

        public bool Interact(Vector2 direction)
        {
            if (CheckNPC(direction))
                return true;
            else if (CheckObject(direction))
                return true;

            return false;
        }

        private bool CheckNPC(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(_transform.position, direction, _interactionDistance, _npcLayerMask);

            if (hit.collider != null)
            {
                hit.collider.GetComponent<IInteractive>()?.Interact(direction);
                return true;
            }

            return false;
        }

        private bool CheckObject(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.CircleCast(_transform.position, _interactionDistance, direction, 0f, _objectLayerMask);

            if (hit.collider != null)
            {
                hit.collider.GetComponent<IInteractive>()?.Interact(direction);
                return true;
            }

            return false;
        }
    }
}