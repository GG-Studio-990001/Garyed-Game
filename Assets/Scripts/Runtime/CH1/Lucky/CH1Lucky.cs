using DG.Tweening;
using Runtime.ETC;
using Runtime.Lucky;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Lucky
{
    public class CH1Lucky : DialogueViewBase
    {
        private DialogueRunner _runner;
        [SerializeField] private GameObject[] _luckys;
        [SerializeField] private LuckyBody _lucky;
        [SerializeField] private RectTransform _bubble;
        [SerializeField] private Vector3[] _leftPositions;
        [SerializeField] private Vector3[] _rightPositions;
        [SerializeField] private Vector3[] _bubblePositions;

        private void Awake()
        {
            _runner = GetComponent<DialogueRunner>();
            _runner.AddCommandHandler("LuckyEnter", LuckyEnter);
            // _runner.AddCommandHandler("LuckyExit", LuckyExit);
            _runner.AddCommandHandler<int>("WalkLeft", WalkLeft);
            _runner.AddCommandHandler<int>("WalkRight", WalkRight);
            _runner.AddCommandHandler("Idle", Idle);
            _runner.AddCommandHandler<bool>("ActiveBubble", ActiveBubble);
            _runner.AddCommandHandler<int>("SetLuckyPos", SetLuckyPos);
            _runner.AddCommandHandler<int>("SetBubblePos", SetBubblePos);

            _runner.AddCommandHandler("ExitFirstMeet", ExitFirstMeet);
        }

        #region Common
        private void LuckyEnter()
        {
            Managers.Data.InGameKeyBinder.PlayerInputDisable();
            for (int i = 0; i < _luckys.Length; i++)
                _luckys[i].SetActive(true);

            Managers.Sound.Play(Sound.LuckyBGM, "[Ch1] Lucky_BGM_4");
        }

        private void LuckyExit()
        {
            Managers.Data.InGameKeyBinder.PlayerInputEnable();
            for (int i = 0; i < _luckys.Length; i++)
                _luckys[i].SetActive(false);

            Managers.Sound.Play(Sound.BGM, "[Ch1] Main_BGM", true);
        }

        private void WalkLeft(int idx)
        {
            _lucky.SetFlipX(false);
            _lucky.Anim.SetAnimation("Walking"); // enum으로 변경
            _lucky.transform.DOLocalMove(_leftPositions[idx], 3f).SetEase(Ease.Linear);
        }

        private void WalkRight(int idx)
        {
            _lucky.SetFlipX(true);
            _lucky.Anim.SetAnimation("Walking");
            _lucky.transform.DOLocalMove(_rightPositions[idx], 3f).SetEase(Ease.Linear);
        }

        private void Idle()
        {
            _lucky.Anim.ResetAnim();
            Debug.Log("Idle");
        }

        private void ActiveBubble(bool active)
        {
            _bubble.gameObject.SetActive(active);
        }
        #endregion

        private void ExitFirstMeet()
        {
            // Exit 호출
            LuckyExit();

            Managers.Data.MeetLucky = true;
            Managers.Data.SaveGame();
        }

        private void SetLuckyPos(int idx)
        {
            switch (idx)
            {
                case 0:
                    _lucky.transform.position = _leftPositions[0];
                    break;
            }
        }

        private void SetBubblePos(int idx)
        {
            _bubble.anchoredPosition = _bubblePositions[idx];
        }
    }
}