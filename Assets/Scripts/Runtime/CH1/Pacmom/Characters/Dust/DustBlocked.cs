using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class DustBlocked : MonoBehaviour
    {
        private Dust _dust;
        private DustRoom _room;
        [SerializeField]
        private InGameDialogue _dialogue;
        [SerializeField]
        private float _reachTime = 0f;
        private bool _dustTalked = false;
        public bool IsBlocked { get; private set; } = false;

        private void Awake()
        {
            _dust = GetComponent<Dust>();
            _room = GetComponent<DustRoom>();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            IsBlocked = true;

            if (collision.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PlayerStr))
            {
                _reachTime += Time.deltaTime;
            }

            if (!_dustTalked && _reachTime > 1.2f)
            {
                if (!_room.IsInRoom && _dust.IsStronger())
                    _dialogue.BlockedDialogue(_dust.DustID);
                _dustTalked = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (_dustTalked)
                _dialogue.StopDialogue(1f);
            
            _reachTime = 0f;
            _dustTalked = false;
            IsBlocked = false;
        }
    }
}