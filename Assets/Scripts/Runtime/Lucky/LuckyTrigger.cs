using Runtime.ETC;
using UnityEngine;
using Yarn.Unity;

public class LuckyTrigger : MonoBehaviour
{
    [SerializeField] private DialogueRunner _dialogueRunner;
    [SerializeField] private GameObject _luckyObj;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Managers.Data.Is3MatchEntered)
            return;

        if (other.CompareTag(GlobalConst.PlayerStr))
        {
            _luckyObj.SetActive(true);
            _dialogueRunner.StartDialogue("Lucky_3Match");
            // TODO: 럭키 전에 이미 한 분기점 추가
        }
    }
}
