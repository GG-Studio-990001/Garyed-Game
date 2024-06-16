using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.CH1.Main
{
    public class PacmomSkip : MonoBehaviour
    {
        private void Update()
        {
            gameObject.SetActive(!(Managers.Data.Scene > 1));
        }

        public void PMSkipBtn()
        {
            Managers.Data.IsPacmomPlayed = true;
            Managers.Data.IsPacmomCleared = true;
            Managers.Data.PacmomCoin = 300;
            Managers.Data.SaveGame();
            SceneManager.LoadScene("CH1");
        }
    }
}