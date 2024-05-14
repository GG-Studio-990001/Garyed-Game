using DG.Tweening;
using Runtime.ETC;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.Luck
{
    public class LuckyDialogue : DialogueViewBase
    {
        private DialogueRunner _runner;
        [SerializeField] private Lucky _lucky;
        [SerializeField] private GameObject _bubble;
        [SerializeField] private Vector3 _outPosition;
        [SerializeField] private Vector3 _inPosition;
        [SerializeField] private GameObject _drawing;
        [SerializeField] private GameObject[] LuckyObjs;
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
            Managers.Sound.Play(Sound.BGM, "[Ch1] Lucky_BGM_03");
        }

        private void LuckyExit()
        {
            Managers.Sound.StopBGM();
            for (int i=0; i< LuckyObjs.Length; i++)
                LuckyObjs[i].SetActive(false);
            _timeline.SetActive(true);
        }

        private void ShowDrawing(bool active)
        {
            if (active)
            {
                Invoke("ActiveDrawing", 1f);
            }
            else
            {
                CancelInvoke("ActiveDrawing");
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