using Runtime.ETC;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.CH1.Main.Stage
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class StageMover : MonoBehaviour
    {
        public UnityEvent OnStageMove;
        [SerializeField] private int moveStageNumber;
        [SerializeField] private Vector2 spawnPosition;
        [SerializeField] private NpcPosition _npcs;

        public Ch1StageChanger StageChanger { get; set; }

        private void Awake()
        {
            if (_npcs == null)
            {
                _npcs = GameObject.Find("NPC").GetComponent<NpcPosition>(); // FindObjectOfType<NpcPosition>();
                if (_npcs == null)
                {
                    Debug.LogError("NpcPosition is not found.");
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GlobalConst.PlayerStr))
            {
                if (moveStageNumber > 3) // 마마고, 3매치 등
                    _npcs.ActiveNpcs(false);
                else
                    _npcs.ActiveNpcs(true);

                StageChanger.SwitchStage(moveStageNumber, spawnPosition);

                OnStageMove?.Invoke();
            }
        }
    }
}
