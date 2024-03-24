using System.Collections;
using TMPro;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PMUI : MonoBehaviour
    {
        public PMData DataController;
        [SerializeField]
        private GameObject[] _pacmomLives = new GameObject[3];
        [SerializeField]
        private TextMeshProUGUI _pacmomScoreTxt;
        [SerializeField]
        private TextMeshProUGUI _rapleyScoreTxt;

        public void LosePacmomLife(int nowLives)
        {
            _pacmomLives[nowLives].SetActive(false);
        }

        public void ShowPacmomScore(int newScore)
        {
            StartCoroutine(nameof(ChangePacmomScore), newScore);
        }

        public void ShowRapleyScore(int newScore)
        {
            StartCoroutine(nameof(ChangeRapleyScore), newScore);
        }

        private IEnumerator ChangePacmomScore(int newScore)
        {
            string scoreStr = _pacmomScoreTxt.text[1..];
            int score = int.Parse(scoreStr);
            float changeTime = DataController.GetChangeTime(newScore - score);

            while (score != newScore)
            {
                score += (score < newScore ? 1 : -1);
                _pacmomScoreTxt.text = "x" + score.ToString();
                yield return new WaitForSeconds(changeTime);
            }
        }

        private IEnumerator ChangeRapleyScore(int newScore)
        {
            string scoreStr = _rapleyScoreTxt.text[1..];
            int score = int.Parse(scoreStr);
            float changeTime = DataController.GetChangeTime(newScore - score);

            while (score != newScore)
            {
                score += (score < newScore ? 1 : -1);
                _rapleyScoreTxt.text = "x" + score.ToString();
                yield return new WaitForSeconds(changeTime);
            }
        }
    }
}