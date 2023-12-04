using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PacmomGameController : MonoBehaviour
    {
        public Rapley rapley;
        public Pacmom pacmom;
        public Transform coins;

        public int rapleyScore { get; private set; }
        public int pacmomScore { get; private set; }
        public int pacmomLives { get; private set; }

        private void Start()
        {
            NewGame();
        }

        private void NewGame()
        {
            SetRapleyScore(0);
            SetPacmomScore(0);
            SetPacmomLives(3);

            foreach (Transform coin in coins)
            {
                coin.gameObject.SetActive(true);
            }

            rapley.ResetState();
            pacmom.ResetState();
        }

        private void SetRapleyScore(int score)
        {
            rapleyScore = score;
        }

        private void SetPacmomScore(int score)
        {
            pacmomScore = score;
        }

        private void SetPacmomLives(int lives)
        {
            pacmomLives = lives;
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
                Debug.Log("라플리 점수: " + rapleyScore);
                Debug.Log("팩맘 점수: " + pacmomScore);
                Debug.Log("Game Clear! 3초 뒤 재시작");

                rapley.gameObject.SetActive(false);
                pacmom.gameObject.SetActive(false);
                Invoke("NewGame", 3f);
            }
        }

        public void RapleyEaten()
        {
            
        }

        public void PacmomEaten()
        {
            // ToDO: 유령한테 죽을 때 처리

            SetPacmomLives(pacmomLives - 1);

            if (pacmomLives > 0)
            {
                Debug.Log("팩맘 목수뮤 -1");
            }
            else
            {
                Debug.Log("팩맘 죽음");
                pacmom.gameObject.SetActive(false);
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
