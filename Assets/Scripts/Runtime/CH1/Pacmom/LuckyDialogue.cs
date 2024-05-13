using DG.Tweening;
using Runtime.Luck;
using UnityEngine;
using Yarn.Unity;
using static PlasticGui.PlasticTableColumn;

namespace Runtime.CH1.Pacmom
{
    public class LuckyDialogue : DialogueViewBase
    {
        private DialogueRunner _runner;
        [SerializeField] private Lucky _lucky;
        [SerializeField] private GameObject _bubble;
        [SerializeField] private Vector3 _outPosition;
        [SerializeField] private Vector3 _inPosition;
        [SerializeField] private GameObject _drawing;

        private void Awake()
        {
            _lucky.transform.localPosition = _outPosition;

            _runner = GetComponent<DialogueRunner>();
            _runner.AddCommandHandler("WalkIn", WalkIn);
            _runner.AddCommandHandler("WalkOut", WalkOut);
            _runner.AddCommandHandler<bool>("ActiveBubble", ActiveBubble);
            _runner.AddCommandHandler("Idle", Idle);
            _runner.AddCommandHandler("Pointing", Pointing);
            _runner.AddCommandHandler("Showing", Showing);
            _runner.AddCommandHandler<bool>("ShowDrawing", ShowDrawing);
        }

        private void ShowDrawing(bool active)
        {
            if (active)
            {
                Invoke("ActiveDrawing", 1f);
            }
            else
            {
                _drawing.SetActive(false);
            }
        }

        private void ActiveDrawing()
        {
            _drawing.SetActive(true);
        }

        private void WalkIn()
        {
            _lucky.Anim.SetAnimation("Walking"); // enum으로 변경
            _lucky.transform.DOLocalMove(_inPosition, 3f).SetEase(Ease.Linear);
            // 여기서 wait 호출 어케하지?
        }

        private void WalkOut()
        {
            _lucky.Anim.SetAnimation("Walking");
            _lucky.SetFlipX(true);
            _lucky.transform.DOLocalMove(_outPosition, 3f).SetEase(Ease.Linear);
        }

        private void Idle()
        {
            _lucky.SetFlipX(false);
            _lucky.Anim.ResetAnim();
            Debug.Log("Idle");
        }

        private void Pointing()
        {
            _lucky.Anim.SetAnimation("Pointing");
        }

        private void Showing()
        {
            _lucky.Anim.SetAnimation("Showing");
        }

        private void ActiveBubble(bool active)
        {
            _bubble.SetActive(active);
        }

        // 팩맘 시작
    }
}