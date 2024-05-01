using Runtime.ETC;
using Runtime.Interface;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Runtime.CH1.SubB
{
    public class Jewelry : MonoBehaviour
    {
        [field: SerializeField] public JewelryType JewelryType { get; set; }
        [SerializeField] private Transform spriteTransform;
        [SerializeField] private float moveTime = 0.5f;
        [SerializeField] private float pushLimitTime = 1.0f;

        public ThreeMatchPuzzleController Controller { get; set; }
        public Tilemap Tilemap { get; set; }

        private Vector3 _orignalPosition;
        private JewelryType _originalType;
        private IMovement _movement;
        private float _pushTime;

        private void Awake()
        {
            Tilemap = GetComponentInParent<Tilemap>();
            _orignalPosition = transform.position;
            _originalType = JewelryType;
            _movement = new JewelryMovement(this.transform, spriteTransform, moveTime, Tilemap);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (JewelryType == JewelryType.None)
            {
                return;
            }

            _pushTime += Time.deltaTime;

            if (other.gameObject.CompareTag(GlobalConst.PlayerStr))
            {
                if (_pushTime > pushLimitTime)
                {
                    _pushTime = 0f;

                    Vector2 playerDetectionPos = new Vector2(other.transform.position.x,
                        other.transform.position.y + other.collider.offset.y);

                    Vector2 direction = (transform.position - (Vector3)playerDetectionPos);

                    direction.x = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? Mathf.Sign(direction.x) : 0f;
                    direction.y = Mathf.Abs(direction.y) > Mathf.Abs(direction.x) ? Mathf.Sign(direction.y) : 0f;

                    if (Controller.ValidateMovement(this, direction))
                    {
                        _movement.Move(direction);
                        Invoke(nameof(CallCheckMatching), moveTime + 0.1f);
                    }
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other) => _pushTime = 0f;

        public void ResetPosition()
        {
            transform.position = _orignalPosition;
            JewelryType = _originalType;
            gameObject.SetActive(true);
        }

        public void DestroyJewelry()
        {
            JewelryType = JewelryType.Disappear;
            Invoke(nameof(SetActiveFalse), 0.1f);
            // 이펙트
        }
        
        private void SetActiveFalse() => gameObject.SetActive(false);

        private void CallCheckMatching() => Controller.CheckMatching();
    }
}
