using Runtime.CH1.Main.Interface;
using Runtime.Interface;
using UnityEngine;

namespace Runtime.CH1.Main.PlayerFunction
{
    public class TopDownInteraction : IInteraction
    {
        private readonly Transform _transform;
        private readonly int _interactionLayerMask;
        private readonly float _interactionDistance;
        
        public TopDownInteraction(Transform transform, int interactionLayerMask, float interactionDistance = 1.0f)
        {
            _transform = transform;
            _interactionLayerMask = interactionLayerMask;
            _interactionDistance = interactionDistance;
        }
        
        public bool Interact(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(_transform.position, direction, _interactionDistance, _interactionLayerMask);
            
            if (hit.collider != null)
            {
                hit.collider.GetComponent<IInteractive>()?.Interact();
                return true;
            }
            
            return false;
        }
    }
}