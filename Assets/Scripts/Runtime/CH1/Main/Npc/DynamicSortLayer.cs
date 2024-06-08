using UnityEngine;

namespace Runtime.CH1.Main.Npc
{
    public class DynamicSortLayer : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            _spriteRenderer.sortingOrder = GetSortingOrder();
        }

        private int GetSortingOrder()
        {
            return (int)(this.transform.position.y * 100) * -1;
        }
    }
}