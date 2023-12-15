using TMPro;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PMUIController : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] pacmomLives = new GameObject[3];
        [SerializeField]
        private TextMeshProUGUI pacmomScore;
        [SerializeField]
        private TextMeshProUGUI rapleyScore;

        public void LosePacmomLife(int nowLives)
        {
            pacmomLives[nowLives].SetActive(false);
        }

        public void ShowPacmomScore(int score)
        {
            pacmomScore.text = score.ToString();
        }

        public void ShowRapleyScore(int score)
        {
            rapleyScore.text = "x" + score.ToString();
        }
    }
}