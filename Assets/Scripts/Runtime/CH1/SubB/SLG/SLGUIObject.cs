using Runtime.CH1.Main.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.CH1.SubB.SLG
{
    public class SlguiObject : MonoBehaviour, IInteractive
    {
        [SerializeField] private Texture2D cursorTexture;
        public UnityEvent onInteract;
        
        public bool Interact(Vector2 direction = default)
        {
            onInteract?.Invoke();
            this.gameObject.SetActive(false);
            
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

            return true;
        }
    }
}