using SLGDefines;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Runtime.CH1.Main.Controller;
using Runtime.ETC;
using Runtime.CH1.Main.Dialogue;
using DG.Tweening;
using Yarn.Unity;

namespace SLGDefines
{ 
    public enum SLGObjectType {
        //Asset
        WOOD = 0,
        STONE = 1,
        ASSETMAX = 2,
        //Window
        ConstructWindow,
        MAX,
    }
}

public class SLGActionComponent : MonoBehaviour
{
    public List<Sprite> SLGPopupSprites;
    [SerializeField] private Texture2D cursorTexture;

    [SerializeField] private GameObject _SLGCanvas;
    [SerializeField] private GameObject _constructUI;
    [SerializeField] private TextMeshProUGUI UI_WoodText;
    [SerializeField] private TextMeshProUGUI UI_StoneText;
    [SerializeField] private TextMeshProUGUI UI_CoinText;
    [SerializeField] private GameObject UI_WoodImg;
    [SerializeField] private GameObject UI_StoneImg;

    [SerializeField] private GameObject Wnd_CostSection;
    [SerializeField] private GameObject Wnd_AccelerateSection;

    [SerializeField] private TextMeshProUGUI Wnd_WoodText;
    [SerializeField] private TextMeshProUGUI Wnd_StoneText;
    [SerializeField] private TextMeshProUGUI Wnd_TimeText;
    [SerializeField] private TextMeshProUGUI Wnd_CoinCostText;
    [SerializeField] private UnityEngine.UI.Button Wnd_ConstructionBtn;
    [SerializeField] private UnityEngine.UI.Button Wnd_AccelerateBtn;
    [SerializeField] private UnityEngine.UI.Button Wnd_CloseBtn;

    [SerializeField] private GameObject _sponSpots;
    [SerializeField] private GameObject SLGConstructionObject;
    [SerializeField] private GameObject SLGTriggerObject;
    [SerializeField] private GameObject SLGConstructionSite;
    [SerializeField] private GameObject SLGMaMagoGate;
    [SerializeField] private GameObject SLGMaMagoGateColider;


    private SLGInteractionObject[] _cachedObjects;
    private int _spawnCount = 0;
    private SLGProgress SLGProgressInfo;
    private long SLGConstructionBeginTime;

    private CH1CommonData CH1Data;

    private int _wood = 0;
    private int _stone = 0;
    private float _spawnTime = 0.0f;

    //CONST Value 
    const int IncreaseAssetCount = 10;
    const int MaxSpawnCount = 3;
    const int NeededAssetCount = 30;
    const int NeededConstrcutionTimeSec = 60 * 60 * 24;
    const float SpawnTime = 3.0f;
    const int NeededCoinCount = 200;

    public bool bShowWnd;

    // 럭키 등장용
    [SerializeField] private DialogueRunner _luckyDialogue;
    // 마마고 상호작용용
    [SerializeField] private Ch1DialogueController _dialogue;

    private void Awake()
    {
        //SLGProgressInfo = SLGProgress.None;
    }

    void Start()
    {
        Managers.Data.LoadGame();
        SLGProgressInfo = Managers.Data.SLGProgressData;
        SLGConstructionBeginTime = Managers.Data.SLGConstructionBeginTime;

        _cachedObjects = _sponSpots.GetComponentsInChildren<SLGInteractionObject>();
        Wnd_ConstructionBtn.onClick.AddListener(OnClickConstructBtn);
        Wnd_CloseBtn.onClick.AddListener(OnClickCloseBtn);
        Wnd_AccelerateBtn.onClick.AddListener(OnClickAccelerateBtn);
        _constructUI.SetActive(false);

        SLGConstructionSite.SetActive(true);
        SLGMaMagoGate.SetActive(false);
        SLGMaMagoGateColider.SetActive(false);
        _sponSpots.SetActive(false);

        GameObject mainObject = FindObjectOfType<Ch1MainSystemController>().gameObject;
        if (mainObject != null)
        {
            CH1Data = mainObject.GetComponent<CH1CommonData>();
        }

        if (SLGProgressInfo == SLGProgress.None)
        {
            SLGTriggerObject.SetActive(true);
            _SLGCanvas.SetActive(false);
        }
        else if (SLGProgressInfo >= SLGProgress.EndConstruction)
        {
            _SLGCanvas.SetActive(false);
            SLGConstructionSite.SetActive(false);
            SLGMaMagoGate.SetActive(true);
            SLGMaMagoGateColider.SetActive(true);
            SLGTriggerObject.SetActive(false);
        }
        else
        {
            SLGTriggerObject.SetActive(false);
            SLGResume();
            InitMap();
            if (SLGProgressInfo == SLGProgress.BeforeConstruction)
            {
                _sponSpots.SetActive(true);
            }
        }
        
    }

    private void Update()
    {
        if (SLGProgressInfo == SLGProgress.Constructing)
        {
            int DurationTimeSec = (int)(((DateTime.Now - DateTime.MinValue).TotalSeconds) - SLGConstructionBeginTime);
            int RemainedTimeSec = NeededConstrcutionTimeSec - DurationTimeSec;
            if (RemainedTimeSec >= 0)
            {
                if (bShowWnd)
                {
                    int Hour = RemainedTimeSec / (60 * 60);
                    RemainedTimeSec -= 60 * 60 * Hour;

                    int Min = RemainedTimeSec / (60);
                    int Sec = RemainedTimeSec % (60);
                    Wnd_TimeText.text = Hour.ToString() + ":" + Min.ToString() + ":" + Sec.ToString();
                }
            }
            else
            {
                EndSLGMode();
            }
        }
        else if(SLGProgressInfo == SLGProgress.BeforeConstruction)
        {
            if(_spawnCount < MaxSpawnCount)
            {
                _spawnTime += Time.deltaTime;
                if(_spawnTime >= SpawnTime)
                {
                    SpawnRandomObject();
                    _spawnTime = 0.0f;
                }
            }
        }
        if(SLGProgressInfo > SLGProgress.ModeOpen && SLGProgressInfo < SLGProgress.ModeClose)
        {
            if(Input.GetMouseButtonUp(0))
            {
                Managers.Sound.Play(Sound.SFX, "SLG/[Ch1] SLG_SFX_Click");
            }
        }
    }

    public bool IsInSLGMode()
    {
        return SLGProgressInfo > SLGProgress.None;
    }

    public Sprite GetInteractionSprite(SLGObjectType InObjectType)
    {
        if ((int)InObjectType >= SLGPopupSprites.Count)
        {
            return null;
        }

        return SLGPopupSprites[(int)InObjectType];
    }

    public void SLGPause()
    {
        _SLGCanvas.SetActive(false);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Managers.Data.SLGProgressData = SLGProgressInfo;
    }

    public void SLGResume()
    {
        _SLGCanvas.SetActive(true);
        SLGTriggerObject.SetActive(false);
        _sponSpots.SetActive(true);
        SLGConstructionObject.SetActive(true);
        _wood = Managers.Data.SLGWoodCount;
        _stone = Managers.Data.SLGStoneCount;

        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        RefreshHudInfo();
    }

    public void RefreshHudInfo()
    {
        RefreshCoinText();
        UI_WoodText.text = _wood.ToString();
        UI_StoneText.text = _stone.ToString();
    }
    public void ProcessObjectInteraction(SLGObjectType InObjectType)
    {
        if (bShowWnd)
        {
            return;
        }
        switch (InObjectType)
        {
            case SLGObjectType.WOOD:
                {
                    _wood += IncreaseAssetCount;
                    UI_WoodText.text = _wood.ToString();
                    _spawnCount--;
                    Managers.Sound.Play(Sound.SFX, "SLG/[Ch1] SLG_SFX_Wood");
                    PlayGainAnim(InObjectType);
                    break;
                }
            case SLGObjectType.STONE:
                {
                    _stone += IncreaseAssetCount;
                    UI_StoneText.text = _stone.ToString();
                    Managers.Sound.Play(Sound.SFX, "SLG/[Ch1] SLG_SFX_Stone");
                    _spawnCount--;
                    PlayGainAnim(InObjectType);
                    break;
                }
            case SLGObjectType.ConstructWindow:
                {
                    _constructUI.SetActive(true);
                    RefreshConstructionWnd();
                    break;
                }
            default: break;
        }
    }

    public void GetSLGPack()
    {
        Managers.Sound.Play(Sound.SFX, "[CH1] SFX_SLG_Get");
    }

    public void OnSLGInit()
    {
        MoveOnNextProgress();
        _SLGCanvas.SetActive(true);
        _sponSpots.SetActive(true);
        SLGTriggerObject.SetActive(false);

        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

        if (SLGConstructionObject != null)
        {
            SLGConstructionObject.SetActive(true);
        }

        _constructUI.SetActive(false);
        RefreshCoinText();

        InitMap();

        _luckyDialogue.StartDialogue("LuckySlg");
        // MoveOnNextProgress(); // 럭키로 이동
    }

    private void InitMap()
    {
        foreach (SLGInteractionObject Object in _cachedObjects)
        {
            Object.gameObject.SetActive(false);
        }
        for (int i = 0; i < Mathf.Min(MaxSpawnCount, _cachedObjects.Length); i++)
        {
            SpawnRandomObject();
        }
    }

    private void SpawnRandomObject()
    {
        if (SLGProgressInfo > SLGProgress.EndConstruction)
        {
            return;
        }

        while (true)
        {
            int RandomNum = (UnityEngine.Random.Range(0, _cachedObjects.Length));
            if (!_cachedObjects[RandomNum]._isActive)
            {
                SLGObjectType NewType = (SLGObjectType)(UnityEngine.Random.Range(0, (int)SLGObjectType.ASSETMAX));
                _cachedObjects[RandomNum].InitInteractionData(NewType);
                _cachedObjects[RandomNum].gameObject.SetActive(true);
                _spawnCount++;
                break;
            }
        }
    }

    private void RefreshConstructionWnd()
    {
        if (_constructUI)
        {
            if (SLGProgressInfo == SLGProgress.BeforeConstruction)
            {
                Wnd_AccelerateSection.gameObject.SetActive(false);
                Wnd_CostSection.gameObject.SetActive(true);

                Wnd_WoodText.text = _wood.ToString() + "/" + NeededAssetCount.ToString();
                Wnd_StoneText.text = _stone.ToString() + "/" + NeededAssetCount.ToString();

                Wnd_WoodText.color = _wood < NeededAssetCount? Color.red : Color.black;
                Wnd_StoneText.color = _stone < NeededAssetCount? Color.red : Color.black;
            }
            else if (SLGProgressInfo == SLGProgress.Constructing)
            {
                Wnd_AccelerateSection.gameObject.SetActive(true);
                Wnd_CostSection.gameObject.SetActive(false);

                Wnd_CoinCostText.text = Managers.Data.PacmomCoin.ToString() + "/" + NeededCoinCount.ToString();
                Wnd_CoinCostText.color = Managers.Data.PacmomCoin < NeededCoinCount ? Color.red : Color.black;
            }

            bShowWnd = true;
        }
    }

    private void RefreshCoinText()
    {
        if (UI_CoinText != null)
        {
            UI_CoinText.text = Managers.Data.PacmomCoin.ToString();
        }
    }
    private void OnClickAccelerateBtn()
    {
        GameObject mainObject = FindObjectOfType<Ch1MainSystemController>().gameObject;
        if (mainObject != null)
        {
            if (Managers.Data.PacmomCoin >= NeededCoinCount)
            {
                Managers.Data.PacmomCoin -= NeededCoinCount;
                RefreshCoinText();
                _dialogue.MamagoThanks(); // 마마고 연출 시작
                _SLGCanvas.SetActive(false);
                bShowWnd = false;
            }
            else
            {
                Debug.Log("건설 불가능");
            }
        }
    }
    private void OnClickCloseBtn()
    {
        if (_constructUI != null)
        {
            _constructUI.SetActive(false);
            bShowWnd = false;
        }
    }
    private void OnClickConstructBtn()
    {
        if (_wood >= NeededAssetCount && _stone >= NeededAssetCount)
        {
            _wood -= NeededAssetCount;
            _stone -= NeededAssetCount;
            RefreshHudInfo();
            MoveOnNextProgress();
        }
        else
        {
            Debug.Log("건설불가능");
        }
    }

    private void PlayGainAnim(SLGObjectType InType)
    {
        //만약 획득 애니메이션 통일해서 쓸거면 이런 애니메이션 관련은 공용함수로 만들어도될듯
        GameObject animTargetImg;
        if (InType == SLGObjectType.WOOD)
        {
            animTargetImg = UI_WoodImg;
        }
        else if(InType ==SLGObjectType.STONE)
        {
            animTargetImg = UI_StoneImg;
        }
        else
        {
            return;
        }

        if(animTargetImg != null)
        {
            RectTransform transform = animTargetImg.GetComponent<RectTransform>();
            if(transform != null)
            {
                transform.localScale = new Vector3(0.7f,0.7f,0.1f);
                transform.DOScale(1.0f, 1.0f).SetEase(Ease.InOutBack);
            }
        }
    }

    private void WriteSLGData()
    {
        Managers.Data.SLGProgressData = SLGProgressInfo;
        Managers.Data.SLGConstructionBeginTime = SLGConstructionBeginTime;
        Managers.Data.SLGWoodCount = _wood;
        Managers.Data.SLGStoneCount = _stone;
    }

    public void MoveOnNextProgress()
    {
        SLGProgressInfo++;

        if (SLGProgressInfo < SLGProgress.EndConstruction)
        {
            if (bShowWnd)
            {
                RefreshConstructionWnd();
            }
            if (SLGProgressInfo == SLGProgress.Constructing)
            {
                SLGConstructionBeginTime = (long)((DateTime.Now - DateTime.MinValue).TotalSeconds);
            }
        }
        else if(SLGProgressInfo == SLGProgress.EndConstruction)
        {
            EndSLGMode();
        }

        WriteSLGData();

        if (SLGProgressInfo != SLGProgress.ModeOpen)
            Managers.Data.SaveGame();
        // SLG팩 획득 ~ 럭키 설명 다 듣기 안하면 SLG 팩 획득 이전으로 돌아가야함
    }

    private void EndSLGMode()
    {
        Debug.Log("건설 끝");
        SLGConstructionObject.SetActive(false);
        _sponSpots.SetActive(false);
        SLGConstructionSite.SetActive(false);
        SLGMaMagoGate.SetActive(true);
        SLGMaMagoGateColider.SetActive(true);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        MoveOnNextProgress();
    }
}