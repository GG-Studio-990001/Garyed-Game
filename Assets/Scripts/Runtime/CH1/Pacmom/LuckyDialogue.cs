using DG.Tweening;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Pacmom
{
    public class LuckyDialogue : DialogueViewBase
    {
        private DialogueRunner _runner;
        [SerializeField] private GameObject _lucky;
        [SerializeField] private GameObject _bubble;
        [SerializeField] private Vector3 _outPosition;
        [SerializeField] private Vector3 _inPosition;

        private void Awake()
        {
            _lucky.transform.localPosition = _outPosition;

            _runner = GetComponent<DialogueRunner>();
            _runner.AddCommandHandler("LuckyWalkIn", LuckyWalkIn);
            _runner.AddCommandHandler("LuckyWalkOut", LuckyWalkOut);
            _runner.AddCommandHandler<bool>("ActiveBubble", ActiveBubble);
        }

        // 럭키 이동 (시작 위치, 끝 위치)

        private void LuckyWalkIn()
        {
            _lucky.transform.DOLocalMove(_inPosition, 3f).SetEase(Ease.Linear);
            // 여기서 wait 호출 어케하지?
        }

        private void LuckyWalkOut()
        {
            _lucky.transform.DOLocalMove(_outPosition, 3f).SetEase(Ease.Linear);
        }

        private void ActiveBubble(bool active)
        {
            _bubble.SetActive(active);
        }

        // 팩맘 시작
    }
}