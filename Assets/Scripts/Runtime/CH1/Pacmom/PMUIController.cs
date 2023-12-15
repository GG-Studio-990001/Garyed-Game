using TMPro;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PMUIController : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] pacmomLives = new GameObject[3];
        [SerializeField]
        private TextMeshProUGUI pacmomScoreTxt;
        [SerializeField]
        private TextMeshProUGUI rapleyScoreTxt;
        [SerializeField]
        private TextMeshProUGUI GameOverTxt;

        public void LosePacmomLife(int nowLives)
        {
            pacmomLives[nowLives].SetActive(false);
        }

        public void ShowPacmomScore(int score)
        {
            pacmomScoreTxt.text = score.ToString();
        }

        public void ShowRapleyScore(int score)
        {
            rapleyScoreTxt.text = "x" + score.ToString();
        }

        public void ShowGameOverUI(string winner)
        {
            GameOverTxt.gameObject.SetActive(true);
            GameOverTxt.text += winner;
        }
    }
}