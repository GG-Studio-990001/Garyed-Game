using DG.Tweening;
using Runtime.ETC;
using Runtime.Lucky;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Lucky
{
    public class LuckyPMDialogue : DialogueViewBase
    {
        private DialogueRunner _runner;
        [SerializeField] private LuckyBody _lucky;
        [SerializeField] private GameObject _bubble;
        [SerializeField] private Vector3 _outPosition;
        [SerializeField] private Vector3 _inPosition;
        [SerializeField] private GameObject _drawing;
        [SerializeField] private GameObject[] _luckyObjs;
        [SerializeField] private GameObject _timeline;

        private void Awake()
        {
            _lucky.transform.localPosition = _outPosition;

            _runner = GetComponent<DialogueRunner>();
            _runner.AddCommandHandler("LuckyEnter", LuckyEnter);
            _runner.AddCommandHandler("LuckyExit", LuckyExit);
            _runner.AddCommandHandler("WalkIn", WalkIn);
            _runner.AddCommandHandler("WalkOut", WalkOut);
            _runner.AddCommandHandler<bool>("ActiveBubble", ActiveBubble);
            _runner.AddCommandHandler("Idle", Idle);
            _runner.AddCommandHandler("Pointing", Pointing);
            _runner.AddCommandHandler("Showing", Showing);
            _runner.AddCommandHandler<bool>("ShowDrawing", ShowDrawing);
        }

        private void Start()
        {
            if (Managers.Data.IsPacmomPlayed)
                LuckyExit();
        }

        private void LuckyEnter()
        {
            Managers.Sound.Play(Sound.LuckyBGM, "[Ch1] Lucky_BGM_4");
        }

        private void LuckyExit()
        {
            Managers.Sound.StopBGM();
            for (int i=0; i< _luckyObjs.Length; i++)
                _luckyObjs[i].SetActive(false);
            _timeline.SetActive(true);
        }

        private void ShowDrawing(bool active)
        {
            if (active)
            {
                Invoke(nameof(ActiveDrawing), 1f);
            }
            else
            {
                CancelInvoke(nameof(ActiveDrawing));
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
    }
}