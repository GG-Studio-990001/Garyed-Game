using Runtime.InGameSystem;
using System.Collections;
using UnityEngine;

public class PMEnding : MonoBehaviour
{
    [SerializeField]
    private SceneSystem sceneSystem;
    [SerializeField]
    private GameObject Timeline_3;
    public bool isGameClear { get; private set; }

    public void RapleyWin()
    {
        Debug.Log("라플리 승리");
        Timeline_3.SetActive(true);
        isGameClear = true;
    }

    public void PacmomWin()
    {
        Debug.Log("팩맘 승리");
        Time.timeScale = 0;
        StartCoroutine("ToMain");
    }

    IEnumerator ToMain()
    {
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;
        sceneSystem.LoadSceneWithFade("Main");
    }
}
