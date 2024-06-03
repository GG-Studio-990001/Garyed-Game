using Runtime.CH1.Main.Player;
using Runtime.CH1.Main.PlayerFunction;
using Runtime.ETC;
using Runtime.Interface;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Runtime.CH1.SubB
{
    public class Jewelry : MonoBehaviour
    {
        [field: SerializeField] public JewelryType JewelryType { get; set; }
        [SerializeField] private float moveTime = 0.5f;
        [SerializeField] private float pushLimitTime = 1.0f;

        public ThreeMatchPuzzleController Controller { get; set; }
        public Tilemap Tilemap { get; set; }
        public bool IsMoving => _movement.IsMoving;

        private Animator _animator;
        private Vector3 _orignalPosition;
        private JewelryType _originalType;
        private JewelryMovement _movement;
        private float _pushTime;
        private TopDownPlayer _player;

        private void Awake()
        {
            Tilemap = GetComponentInParent<Tilemap>();
            _animator = GetComponent<Animator>();
            _orignalPosition = transform.position;
            _originalType = JewelryType;
            _movement = new JewelryMovement(this.transform, moveTime, Tilemap);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(GlobalConst.PlayerStr))
            {
                _player = other.gameObject.GetComponent<TopDownPlayer>();
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (JewelryType == JewelryType.None || JewelryType == JewelryType.Disappear)
            {
                return;
            }

            if (other.gameObject.CompareTag(GlobalConst.PlayerStr))
            {
                _pushTime += Time.deltaTime;
                
                if (_pushTime > pushLimitTime)
                {
                    _pushTime = 0f;

                    Vector2 playerDetectionPos = new Vector2(other.transform.position.x,
                        other.transform.position.y + other.collider.offset.y);

                    Vector2 direction = (transform.position - (Vector3)playerDetectionPos);

                    direction.x = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? Mathf.Sign(direction.x) : 0f;
                    direction.y = Mathf.Abs(direction.y) > Mathf.Abs(direction.x) ? Mathf.Sign(direction.y) : 0f;
                    
                    if (direction != _player.Direction)
                    {
                        return;
                    }

                    if (Controller.ValidateMovement(this, direction))
                    {
                        _movement.Move(direction);
                        Invoke(nameof(CallCheckMatching), moveTime + 0.1f);
                    }
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(GlobalConst.PlayerStr))
            {
                _pushTime = 0f;
                _player = null;
            }
        }

        public void ResetPosition()
        {
            transform.position = _orignalPosition;
            JewelryType = _originalType;
            gameObject.SetActive(true);
        }

        public void DestroyJewelry()
        {
            if (JewelryType is not JewelryType.Disappear && _animator != null)
            {
                _animator.Play($"Type{JewelryType}_Effect");
            }
            
            JewelryType = JewelryType.Disappear;
            
            Invoke(nameof(SetActiveFalse), 1f);
        }

        private void SetActiveFalse()
        {
            gameObject.SetActive(false);
            transform.position = new Vector3(-100, -100, 0);
        }

        private void CallCheckMatching() => Controller.CheckMatching();
        public void PlayEffectSound() => Managers.Sound.Play(Sound.SFX, "[CH1] Candy_SFX_Meow");
    }
}
