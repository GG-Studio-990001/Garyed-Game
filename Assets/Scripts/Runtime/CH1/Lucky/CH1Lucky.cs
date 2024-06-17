using DG.Tweening;
using Runtime.ETC;
using Runtime.Lucky;
using System;
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
            _runner.AddCommandHandler("Exit3Match", Exit3Match);
            _runner.AddCommandHandler("ExitSLG", ExitSLG);
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            onDialogueLineFinished();
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

            Managers.Data.SaveGame();
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

        #region 상황 따라 다르게
        private void SetLuckyPos(int idx)
        {
            switch (idx)
            {
                case 0:
                    _lucky.transform.position = _leftPositions[0];
                    break;
                case 1:
                case 2:
                case 3:
                    _lucky.transform.position = _rightPositions[idx];
                    break;
            }
        }

        private void SetBubblePos(int idx)
        {
            _bubble.anchoredPosition = _bubblePositions[idx];
        }
        #endregion

        #region Exit
        // LuckyExit() 호출 필수

        private void ExitFirstMeet()
        {
            LuckyExit();

            Managers.Data.MeetLucky = true;
            Managers.Data.SaveGame();

            Managers.Sound.Play(Sound.BGM, "[Ch1] Main_BGM", true);
        }

        private void Exit3Match()
        {
            LuckyExit();

            Managers.Sound.Play(Sound.BGM, "[Ch1] Main(Cave)_BGM", true);
        }

        private void ExitSLG()
        {
            LuckyExit();

            SLGActionComponent slgAction = FindObjectOfType<SLGActionComponent>();
            if (slgAction != null)
            {
                slgAction.MoveOnNextProgress();
            }

            Managers.Sound.Play(Sound.BGM, "[Ch1] Main_BGM", true);
        }
        #endregion

        /*
        public void S1ExplainStart()
        {
            if (Managers.Data.MeetLucky && !Managers.Data.Is3MatchEntered)
            {
                _lucky.transform.localPosition = _outPosition[0];
                _runner.StartDialogue("Lucky_3Match");
            }
        }

        public void S3ExplainStart()
        {
            // 3매치 퍼즐 클리어 저장 변수 없나? => 생기면 조건문 수정
            if (Managers.Data.MeetLucky && Managers.Data.Scene <= 3 && Managers.Data.SceneDetail <= 0) // 3.0 넘었다면 이미 깬 것
            {
                _lucky.transform.localPosition = _outPosition[1];
                _runner.StartDialogue("Lucky_3Match_Stage3");
            }
        }

        private void ActiveFish()
        {
            _fish.SetActive(true);
        }

        private void ExplodeFish()
        {
            _3MatchController.CheckMatching();
        }

        private void ExplaneDone()
        {
            Managers.Data.Is3MatchEntered = true;
            Managers.Data.SaveGame();
        }
        */
    }
}