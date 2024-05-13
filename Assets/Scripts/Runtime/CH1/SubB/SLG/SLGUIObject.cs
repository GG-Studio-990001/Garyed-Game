using Runtime.CH1.Main.Interface;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace Runtime.CH1.SubB.SLG
{
    public class SlguiObject : MonoBehaviour, IInteractive
    {
        public UnityEvent onInteract;

        public bool Interact(Vector2 direction = default)
        {
            onInteract?.Invoke();

            SLGActionComponent SLGAction = FindObjectOfType<SLGActionComponent>();
            if (SLGAction != null)
            {
                SLGAction.OnSLGInit();
            }

            return true;
        }
    
    }
}