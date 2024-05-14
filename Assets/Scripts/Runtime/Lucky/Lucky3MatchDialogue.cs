using DG.Tweening;
using Runtime.CH1.Main.Player;
using Runtime.ETC;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.Luck
{
    public class Lucky3MatchDialogue : DialogueViewBase
    {
        private DialogueRunner _runner;
        [SerializeField] private Lucky _lucky;
        [SerializeField] private GameObject _bubble;
        [SerializeField] private Vector3 _outPosition;
        [SerializeField] private Vector3 _inPosition;
        // [SerializeField] private GameObject _drawing;
        [SerializeField] private GameObject[] LuckyObjs;
        // [SerializeField] private GameObject _timeline;
        [SerializeField] private TopDownPlayer _player;

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
            _runner.AddCommandHandler("ExplaneDone", ExplaneDone);
        }

        private void Start()
        {
            if (Managers.Data.IsPacmomPlayed)
                LuckyExit();
        }

        private void ExplaneDone()
        {
            Managers.Data.Is3MatchEntered = true;
            Managers.Data.SaveGame();
        }

        private void LuckyEnter()
        {
            // 플레이어 멈춰
            Managers.Data.InGameKeyBinder.PlayerInputDisable();
            Managers.Sound.Play(Sound.BGM, "[Ch1] Lucky_BGM_03");
        }

        private void LuckyExit()
        {
            Managers.Data.InGameKeyBinder.PlayerInputEnable();
            Managers.Sound.StopBGM();
            for (int i = 0; i < LuckyObjs.Length; i++)
                LuckyObjs[i].SetActive(false);
        }

        private void WalkIn()
        {
            _lucky.Anim.SetAnimation("Walking"); // enum으로 변경
            _lucky.transform.DOLocalMove(_inPosition, 3f).SetEase(Ease.Linear);
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

        private void ActiveBubble(bool active)
        {
            _bubble.SetActive(active);
        }
    }
}