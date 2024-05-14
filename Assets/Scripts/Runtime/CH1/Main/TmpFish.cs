using UnityEngine;

public class TmpFish : MonoBehaviour
{
    // Fish가 3매치컨트롤러 배열에 들어가게하려는 임시 클래스

    void Start()
    {
        Invoke("InActive", 1f);
    }

    private void InActive()
    {
        this.gameObject.SetActive(false);
    }
}
