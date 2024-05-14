using Runtime.CH1.Main.Player;
using Runtime.CH1.Main;
using UnityEngine;
using DG.Tweening;
using Runtime.ETC;

public class CutSceneDialogue : MonoBehaviour
{
    [Header("=CutScene=")]
    [SerializeField] private GameObject _illerstrationParent;
    [SerializeField] private GameObject[] _illerstration = new GameObject[1];
    [SerializeField] private GameObject _lucky;
    [Header("=Player=")]
    [SerializeField] public TopDownPlayer Player;
    [SerializeField] private Vector3 _location;
    [Header("=Npc=")]
    [SerializeField] private Npc[] _npc = new Npc[3];
    [SerializeField] private Vector3[] _locations = new Vector3[3];

    #region Character Anim
    public void NpcJump(int idx)
    {
        Vector3 nowPos = _npc[idx].transform.position;

        _npc[idx].transform.DOJump(nowPos, 0.3f, 1, 0.4f).SetEase(Ease.Linear);
    }

    public void CharactersMove1()
    {
        string state = PlayerState.Move.ToString();

        Player.Animation.SetAnimation(state, Vector2.right);
        Player.transform.DOMove(_location, 5f).SetEase(Ease.Linear);

        for (int i = 0; i < _npc.Length; i++)
        {
            _npc[i].Anim.SetAnimation(state, Vector2.right);
            _npc[i].transform.DOMove(_locations[i], 5f).SetEase(Ease.Linear);
        }
    }

    public void CharactersStop1()
    {
        string state = PlayerState.Idle.ToString();

        Player.Animation.SetAnimation(state, Vector2.down);

        _npc[0].Anim.SetAnimation(state, Vector2.right);
        _npc[1].Anim.SetAnimation(state, Vector2.up);
        _npc[2].Anim.SetAnimation(state, Vector2.left);
    }
    #endregion

    #region else
    public void GetLucky()
    {
        Managers.Sound.Play(Sound.SFX, "[Ch1] Lucky_SFX_Dog&Key");
        _lucky.SetActive(false);
        Managers.Data.MeetLucky = true;
        Managers.Data.SaveGame();
    }

    public void ShowIllustration(int num)
    {
        _illerstrationParent.SetActive(true);

        for (int i = 0; i < _illerstration.Length; i++)
        {
            if (i == num)
                _illerstration[i].SetActive(true);
            else
                _illerstration[i].SetActive(false);
        }
    }

    public void HideIllustration()
    {
        _illerstrationParent.SetActive(false);
    }
    #endregion

}
