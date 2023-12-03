using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PacmomGameController : MonoBehaviour
    {
        public Rapley rapley;
        public Transform coins;

        public int rapleyScore { get; private set; }

        private void Start()
        {
            NewGame();
        }

        private void NewGame()
        {
            SetRapleyScore(0);

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

        public void CoinEaten(Coin coin)
        {
            coin.gameObject.SetActive(false);
            SetRapleyScore(rapleyScore + 1);

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
