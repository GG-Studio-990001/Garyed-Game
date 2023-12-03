using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PacmomGameController : MonoBehaviour
    {
        public Rapley rapley;
        public Transform coins;

        public int rapleyScore { get; private set; }
        public int pacmomScore { get; private set; }

        private void Start()
        {
            NewGame();
        }

        private void NewGame()
        {
            SetRapleyScore(0);
            SetPacmomScore(0);

            foreach (Transform coin in coins)
            {
                coin.gameObject.SetActive(true);
            }

            rapley.ResetState();
        }

        private void SetRapleyScore(int score)
        {
            rapleyScore = score;
        }

        private void SetPacmomScore(int score)
        {
            pacmomScore = score;
        }

        public void CoinEaten(Coin coin, string who)
        {
            coin.gameObject.SetActive(false);

            if (who == "Rapley")
                SetRapleyScore(rapleyScore + 1);
            else if (who == "Pacmom")
                SetPacmomScore(pacmomScore + 1);

            if (!HasRemainingCoins())
            {
                Debug.Log("Game Clear! 3초 뒤 재시작");
                rapley.gameObject.SetActive(false);
                Invoke("NewGame", 3f);
            }
        }

        private bool HasRemainingCoins()
        {
            foreach (Transform coin in coins)
            {
                if (coin.gameObject.activeSelf)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
