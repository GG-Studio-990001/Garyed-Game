using DG.Tweening;
using Runtime.CH1.Main.Player;
using Runtime.CH1.SubB;
using Runtime.ETC;
using Runtime.Lucky;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Lucky
{
    public class LuckyCH1Dialogue : DialogueViewBase
    {
        // 클래스 이름 수정해야함
        private DialogueRunner _runner;
        [SerializeField] private GameObject _luckyLayer;
        [SerializeField] private LuckyBody _lucky;
        [SerializeField] private GameObject _bubble;
        [SerializeField] private Vector3[] _inPosition;
        [SerializeField] private Vector3[] _outPosition;
        [SerializeField] private TopDownPlayer _player;
        [SerializeField] private GameObject _fish;
        [SerializeField] private ThreeMatchPuzzleController _3MatchController;
        [SerializeField] private SLGActionComponent _slgAction;

        private void Awake()
        {
            _runner = GetComponent<DialogueRunner>();
            _runner.AddCommandHandler("LuckyEnter", LuckyEnter);
            _runner.AddCommandHandler<int>("WalkIn", WalkIn);
            _runner.AddCommandHandler<int>("WalkOut", WalkOut);
            _runner.AddCommandHandler<bool>("ActiveBubble", ActiveBubble);
            _runner.AddCommandHandler("Idle", Idle);
            _runner.AddCommandHandler("ExplaneDone", ExplaneDone);
            _runner.AddCommandHandler("ActiveFish", ActiveFish);
            _runner.AddCommandHandler("ExplodeFish", ExplodeFish);
            _runner.AddCommandHandler("SLGExplaneDone", SLGExplaneDone);
            _runner.AddCommandHandler("LuckyExit3Match", LuckyExit3Match);
            _runner.AddCommandHandler("LuckyExitSLG", LuckyExitSLG);
        }

        public void SLGExplainStart()
        {
            if (Managers.Data.SLGProgressData == SLGProgress.ModeOpen)
            {
                _lucky.transform.localPosition = _outPosition[2];
                _runner.StartDialogue("Lucky_SLG");
            }
        }

        private void SLGExplaneDone()
        {
            _slgAction.MoveOnNextProgress();
            Managers.Data.SaveGame();
        }

        #region 3 매치 & 공용
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

        private void LuckyEnter()
        {
            // 플레이어 멈춰
            Managers.Data.InGameKeyBinder.PlayerInputDisable();
            Managers.Sound.Play(Sound.LuckyBGM, "[Ch1] Lucky_BGM_4");
        }

        private void LuckyExit()
        {
            Managers.Data.InGameKeyBinder.PlayerInputEnable();
            _luckyLayer.SetActive(false);
            Idle();
            Managers.Sound.StopBGM();
        }

        private void LuckyExit3Match()
        {
            LuckyExit();
            Managers.Sound.Play(Sound.BGM, "[Ch1] Main(Cave)_BGM", true);
        }

        private void LuckyExitSLG()
        {
            LuckyExit();
            Managers.Sound.Play(Sound.BGM, "[Ch1] Main_BGM", true);
        }

        private void WalkIn(int i)
        {
            _lucky.Anim.SetAnimation("Walking"); // enum으로 변경
            _lucky.transform.DOLocalMove(_inPosition[i], 3f).SetEase(Ease.Linear);
        }

        private void WalkOut(int i)
        {
            _lucky.Anim.SetAnimation("Walking");
            _lucky.SetFlipX(true);
            _lucky.transform.DOLocalMove(_outPosition[i], 3f).SetEase(Ease.Linear);
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
        #endregion
    }
}