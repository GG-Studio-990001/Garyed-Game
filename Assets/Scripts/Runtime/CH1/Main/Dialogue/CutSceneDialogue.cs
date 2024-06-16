using Runtime.CH1.Main.Player;
using UnityEngine;
using DG.Tweening;
using Runtime.ETC;
using UnityEngine.SceneManagement;
using Runtime.CH1.Main.Npc;
using Yarn.Unity;

namespace Runtime.CH1.Main.Dialogue
{
    public class CutSceneDialogue : MonoBehaviour
    {
        [Header("=Player=")]
        public TopDownPlayer Player;
        [SerializeField] private Vector3[] _location = new Vector3[2];
        [Header("=Npc=")]
        public NpcPosition NpcPos;
        [SerializeField] private NpcBody[] _npc = new NpcBody[3];
        [SerializeField] private Vector3 _r2monLocation;
        [SerializeField] private NpcBody _mamago;
        [SerializeField] private Vector3[] _mamagoLocation;
        [Header("=Else=")]
        [SerializeField] private GameObject _illerstration;
        [SerializeField] private GameObject _lucky;
        [SerializeField] private GameObject _stage2;
        [SerializeField] private BridgeController _bridge;
        [SerializeField] private DialogueRunner _luckyDialogue;
        private Sequence _shakeTween;

        public void SetR2MonPosition()
        {
            _npc[2].transform.position = _r2monLocation;
        }

        public void ChangeChapter2()
        {
            SceneManager.LoadScene("CH2");
        }

        public void BuildCompany()
        {
            Managers.Sound.Play(Sound.SFX, "[CH1] Mamago_Construction");
            SLGActionComponent slgAction = FindObjectOfType<SLGActionComponent>();
            if (slgAction != null)
            {
                slgAction.MoveOnNextProgress();
            }
        }

        public void CompleteSFX()
        {
            Managers.Sound.Play(Sound.SFX, "[CH1] Mamago_Complete");
        }

        public void MamagoJump()
        {
            Vector3 nowPos = _mamago.transform.position;
            _mamago.transform.DOJump(nowPos, 0.3f, 1, 0.4f).SetEase(Ease.Linear);
        }

        public void MamagoMove1()
        {
            _mamago.Anim.SetAnimation(GlobalConst.MoveStr, Vector2.right);
            _mamago.transform.DOMove(_mamagoLocation[0], 3f).SetEase(Ease.Linear);
        }

        public void MamagoMove2()
        {
            _mamago.Anim.SetAnimation(GlobalConst.MoveStr, Vector2.up);
            _mamago.transform.DOMove(_mamagoLocation[1], 1f).SetEase(Ease.Linear);
        }

        public void MamagoEnter()
        {
            _mamago.gameObject.SetActive(false);
        }

        public void SetNpcPosition(int i)
        {
            if (i == 0) // 3매치 깬 후 위치
            {
                NpcPos.SetNpcPosition(6);
            }
            else if (i == 1) // 맵3 위치
            {
                NpcPos.SetNpcPosition(8);
            }
        }

        public void BreakBridge()
        {
            Managers.Sound.StopSFX();
            Managers.Sound.Play(Sound.SFX, "[Ch1] SFX_Explosion");
            _bridge.ActiveBrokenBridge();
        }

        public void ShakeMap(bool shake)
        {
            if (shake)
            {
                _shakeTween = DOTween.Sequence();
                _shakeTween.Append(_stage2.transform.DOShakePosition(5000f, new Vector3(0.1f, 0.1f, 0)));
                Managers.Sound.Play(Sound.SFX, "[Ch1] SFX_Before explosion");
            }
            else
            {
                _shakeTween.Kill();
            }
        }

        #region Character Anim
        public void NpcJump(int idx)
        {
            Vector3 nowPos = _npc[idx].transform.position;

            _npc[idx].transform.DOJump(nowPos, 0.3f, 1, 0.4f).SetEase(Ease.Linear);
        }

        public void CharactersMove(int num)
        {
            switch (num)
            {
                case 0:
                    CharactersMove0();
                    break;
                case 1:
                    CharactersMove1();
                    break;
                case 2:
                    CharactersMove2();
                    break;
                case 3:
                    CharactersMove3();
                    break;
                case 4:
                    CharactersMove4();
                    break;
                default:
                    Debug.LogError("Invalid Move Number");
                    break;
            }
        }

        private void CharactersMove4()
        {
            // 라플리 빼고 맵3으로
            for (int i = 0; i < _npc.Length; i++)
            {
                _npc[i].Anim.SetAnimation(GlobalConst.MoveStr, Vector2.right);
                _npc[i].transform.DOMove(NpcPos.NpcLocations[i].Locations[7], 5f).SetEase(Ease.Linear);
            }
        }

        private void CharactersMove3()
        {
            // 라플리 빼고 다리 건너기
            for (int i = 0; i < _npc.Length; i++)
            {
                _npc[i].Anim.SetAnimation(GlobalConst.MoveStr, Vector2.right);
                _npc[i].transform.DOMove(NpcPos.NpcLocations[i].Locations[5], 5f).SetEase(Ease.Linear);
            }
        }

        private void CharactersMove2()
        {
            // 라플리 빼고 동굴 앞으로 이동
            for (int i = 0; i < _npc.Length; i++)
            {
                _npc[i].Anim.SetAnimation(GlobalConst.MoveStr, Vector2.right);
                _npc[i].transform.DOMove(NpcPos.NpcLocations[i].Locations[4], 5f).SetEase(Ease.Linear);
            }
        }

        private void CharactersMove1()
        {
            Player.Animation.SetAnimation(GlobalConst.MoveStr, Vector2.right);
            Player.transform.DOMove(_location[0], 5f).SetEase(Ease.Linear);

            for (int i = 0; i < _npc.Length; i++)
            {
                _npc[i].Anim.SetAnimation(GlobalConst.MoveStr, Vector2.right);
                _npc[i].transform.DOMove(NpcPos.NpcLocations[i].Locations[2], 5f).SetEase(Ease.Linear);
            }
        }

        private void CharactersMove0()
        {
            // 라플리 빼고 오른쪽으로 이동

            for (int i = 0; i < _npc.Length; i++)
            {
                _npc[i].Anim.SetAnimation(GlobalConst.MoveStr, Vector2.right);
                _npc[i].transform.DOMove(NpcPos.NpcLocations[i].Locations[1], 5f).SetEase(Ease.Linear);
            }
        }

        public void CharactersStop(int num)
        {
            // TODO: 리팩터링 필요 너무 막 짬
            switch (num)
            {
                case 0:
                    PlayerIdleRight();
                    break;
                case 1:
                    CharIdleCenter();
                    break;
                case 2:
                    CharIdleLeft();
                    break;
                case 3:
                    CharIdleDown();
                    break;
                default:
                    Debug.LogError("Invalid Move Number");
                    break;
            }
        }

        private void CharIdleDown()
        {
            for (int i = 0; i < 3; i++)
            {
                _npc[i].Anim.SetAnimation(GlobalConst.IdleStr, Vector2.down);
            }
        }

        private void CharIdleLeft()
        {
            for (int i = 0; i < 3; i++)
            {
                _npc[i].Anim.SetAnimation(GlobalConst.IdleStr, Vector2.left);
            }
        }

        private void CharIdleCenter()
        {
            Player.Animation.SetAnimation(GlobalConst.IdleStr, Vector2.down);

            _npc[0].Anim.SetAnimation(GlobalConst.IdleStr, Vector2.right);
            _npc[1].Anim.SetAnimation(GlobalConst.IdleStr, Vector2.up);
            _npc[2].Anim.SetAnimation(GlobalConst.IdleStr, Vector2.left);
        }

        public void PlayerIdleRight()
        {
            Player.Animation.SetAnimation(GlobalConst.IdleStr, Vector2.right);
        }

        public void SetPlayerPosition(int idx)
        {
            Player.transform.position = _location[idx];
        }
        #endregion

        #region Else
        public void GetLucky()
        {
            Managers.Sound.Play(Sound.SFX, "[Ch1] Lucky_SFX_Dog&Key");
            _lucky.SetActive(false);
        }

        public void MeetLucky()
        {
            // Find로 변경?
            _luckyDialogue.StartDialogue("LuckyFirstMeet");
        }

        public void ShowIllustration(bool show)
        {
            _illerstration.SetActive(show);
        }
        #endregion

    }
}