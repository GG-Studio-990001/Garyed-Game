using System.Collections;
using UnityEngine;
using Sound = Runtime.ETC.Sound;

namespace Runtime.CH1.Pacmom
{
    public class PMData : MonoBehaviour
    {
        #region 선언
        private PMGameController _gameController;

        [Header("=Contoller=")]
        [SerializeField]
        private PMUI _uiController;
        [SerializeField]
        private PMEnding _ending;

        [Header("=Item=")]
        [SerializeField]
        private Transform _coins;
        [SerializeField]
        private Transform _vacuums;

        private int _rapleyScore;
        private int _pacmomScore;
        private int _pacmomLives;
        private readonly float _normalWaitTime = 0.03f;
        private readonly float _fasterWaitTime = 0.02f;
        #endregion

        #region Awake & Start
        private void Awake()
        {
            _gameController = GetComponent<PMGameController>();
            _uiController.DataController = this;

            foreach (Transform coin in _coins)
            {
                coin.GetComponent<Coin>().GameController = _gameController;
            }

            foreach (Transform vacuum in _vacuums)
            {
                vacuum.GetComponent<Vacuum>().GameController = _gameController;
            }
        }

        private void Start()
        {
            SetRapleyScore(0);
            SetPacmomScore(0);
            SetPacmomLives(3);
        }
        #endregion

        #region Score & Lives
        private void SetRapleyScore(int score)
        {
            _rapleyScore = score;
            _uiController.ShowRapleyScore(score);
        }

        private void SetPacmomScore(int score)
        {
            _pacmomScore = score;
            _uiController.ShowPacmomScore(score);
        }

        private void SetPacmomLives(int lives)
        {
            if (lives < 0)
                return;

            _pacmomLives = lives;
        }

        public void RapleyScore1Up()
        {
            SetRapleyScore(_rapleyScore + 1);
        }

        public void PacmomScore1Up()
        {
            SetPacmomScore(_pacmomScore + 1);
        }

        public void LosePacmomLife()
        {
            SetPacmomLives(_pacmomLives - 1);
            _uiController.LosePacmomLife(_pacmomLives);
        }

        public bool IsPacmomAlive()
        {
            return (_pacmomLives > 0);
        }
        #endregion

        #region About Coins
        public void TakeHalfCoins(bool isRapleyTake)
        {
            if (isRapleyTake)
            {
                int score = _pacmomScore / 2;
                SetRapleyScore(_rapleyScore + score);
                SetPacmomScore(_pacmomScore - score);
            }
            else
            {
                int score = _rapleyScore / 2;
                SetPacmomScore(_pacmomScore + score);
                SetRapleyScore(_rapleyScore - score);
            }
        }

        public float GetChangeTime(int diff)
        {
            diff = Mathf.Abs(diff);

            if (diff >= 50)
                return _fasterWaitTime;
            else
                return _normalWaitTime;
        }

        private float GetCoinTime(int score)
        {
            if (score >= 100)
                return _fasterWaitTime;
            else
                return _normalWaitTime;
        }

        public IEnumerator ReleaseHalfCoins()
        {
            // 방에서 나오는 먼지 예외처리
            StopAllCoroutines();

            int score = _pacmomScore / 2;
            SetPacmomScore(_pacmomScore - score);

            float releaseTime = GetCoinTime(score);

            while (score > 0)
            {
                int rand = Random.Range(0, _coins.childCount);
                Transform childCoin = _coins.transform.GetChild(rand);
                Coin coin = childCoin.GetComponent<Coin>();

                if (!coin.gameObject.activeSelf)
                {
                    coin.ResetCoin();
                    score--;
                    yield return new WaitForSeconds(releaseTime);
                }
            }

            _gameController.AfterPacmomEatenByDust();
        }

        public IEnumerator GetRemaningCoins()
        {
            foreach (Transform coin in _coins)
            {
                if (coin.gameObject.activeSelf)
                {
                    Managers.Sound.Play(Sound.Effect, "Pacmom_SFX_10");

                    SetRapleyScore(_rapleyScore + 1);
                    coin.gameObject.SetActive(false);
                    yield return new WaitForSeconds(_normalWaitTime);
                }
            }
            Invoke(nameof(ChooseAWinner), 1.5f);
        }

        private void ChooseAWinner()
        {
            Managers.Sound.StopEffect();
            _gameController.DialogueStop();

            if (_rapleyScore > _pacmomScore)
                _ending.RapleyWin();
            else
                _ending.PacmomWin();
        }

        public bool HasRemainingCoins()
        {
            foreach (Transform coin in _coins)
            {
                if (coin.gameObject.activeSelf)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}