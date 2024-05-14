using SLGDefines;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Runtime.CH1.Main.Stage;
using Runtime.CH1.Main.Controller;
using Runtime.ETC;
using Runtime.CH1.Main.Dialogue;

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

    //CONST Value 
    const int INCREASE_ASSET_COUNT = 10;
    const int MAX_SPAWN_COUNT = 3;
    const int NEEDED_ASSET_COUNT = 30;
    const int NEEDED_CONSTRUCTION_TIME_SEC = 60 * 60 * 24;
    const int SCENE_MOVE_COUNT_SPAWN = 5;
    const int NEEDED_COIN_COUNT = 200;

    public bool bShowWnd;

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

        Ch1StageController SC = FindObjectOfType<Ch1StageController>();
        if (SC != null)
        {
            SC.StageChanger.OnStageEnd += OnChangeStageForSLG;
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
            int RemainedTimeSec = NEEDED_CONSTRUCTION_TIME_SEC - DurationTimeSec;
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
                    _wood += INCREASE_ASSET_COUNT;
                    UI_WoodText.text = _wood.ToString();
                    break;
                }
            case SLGObjectType.STONE:
                {
                    _stone += INCREASE_ASSET_COUNT;
                    UI_StoneText.text = _stone.ToString();
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
        Debug.Log("럭키 등장");
        // MoveOnNextProgress();
    }

    private void InitMap()
    {
        foreach (SLGInteractionObject Object in _cachedObjects)
        {
            Object.gameObject.SetActive(false);
        }
        for (int i = 0; i < Mathf.Min(MAX_SPAWN_COUNT, _cachedObjects.Length); i++)
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

                Wnd_WoodText.text = _wood.ToString() + "/" + NEEDED_ASSET_COUNT.ToString();
                Wnd_StoneText.text = _stone.ToString() + "/" + NEEDED_ASSET_COUNT.ToString();
            }
            else if (SLGProgressInfo == SLGProgress.Constructing)
            {
                Wnd_AccelerateSection.gameObject.SetActive(true);
                Wnd_CostSection.gameObject.SetActive(false);

                Wnd_CoinCostText.text = Managers.Data.PacmomCoin.ToString() + "/" + NEEDED_COIN_COUNT.ToString();
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
            if (Managers.Data.PacmomCoin >= NEEDED_COIN_COUNT)
            {
                Managers.Data.PacmomCoin -= NEEDED_COIN_COUNT;
                RefreshCoinText();
                MoveOnNextProgress();
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
        //TODO 건설 가능 시 텍스트 색 표시 수정
        if (_wood >= NEEDED_ASSET_COUNT && _stone >= NEEDED_ASSET_COUNT)
        {
            _wood -= NEEDED_ASSET_COUNT;
            _stone -= NEEDED_ASSET_COUNT;
            RefreshHudInfo();
            MoveOnNextProgress();
        }
        else
        {
            Debug.Log("건설불가능");
        }
    }

    private void OnChangeStageForSLG()
    {
        if (CH1Data != null && SLGProgressInfo == SLGProgress.BeforeConstruction)
        {
            CH1Data._sceneMoveCount++;
            if (CH1Data._sceneMoveCount >= SCENE_MOVE_COUNT_SPAWN)
            {
                SpawnRandomObject();
                CH1Data._sceneMoveCount = 0;
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

    private void MoveOnNextProgress()
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
        else
        {
            EndSLGMode();
        }

        WriteSLGData();
        Managers.Data.SaveGame();
    }

    private void EndSLGMode()
    {
        Debug.Log("건설 끝");
        _SLGCanvas.SetActive(false);
        bShowWnd = false;
        SLGConstructionObject.SetActive(false);
        _sponSpots.SetActive(false);
        SLGConstructionSite.SetActive(false);
        SLGMaMagoGate.SetActive(true);
        SLGMaMagoGateColider.SetActive(true);
    }
}