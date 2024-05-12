using SLGDefines;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Runtime.CH1.Main.Stage;
using Runtime.CH1.Main.Controller;
using Runtime.ETC;

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


    private SLGInteractionObject[] _cachedObjects;
    private int _spawnCount = 0;
    private SLGProgress SLGProgressInfo;
    private int SLGConstructionBeginTime;

    private CH1CommonData CH1Data;

    //Data 파일로 따로 분리?
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

    void Start()
    {
        SLGProgressInfo = Managers.Data.SLGProgressData;
        SLGConstructionBeginTime = Managers.Data.SLGConstructionBeginTime;

        _sponSpots.SetActive(false);
        _cachedObjects = _sponSpots.GetComponentsInChildren<SLGInteractionObject>();
        Wnd_ConstructionBtn.onClick.AddListener(OnClickConstructBtn);
        Wnd_CloseBtn.onClick.AddListener(OnClickCloseBtn);
        Wnd_AccelerateBtn.onClick.AddListener(OnClickAccelerateBtn);
    }

    private void Update()
    {
        if(bShowWnd)
        {
            RefreshTimeText();
        }
    }
    
    public Sprite GetInteractionSprite (SLGObjectType InObjectType)
    {
        if((int)InObjectType >=  SLGPopupSprites.Count) 
        { 
            return null; 
        }

        return SLGPopupSprites[(int)InObjectType];
    }

    public void ProcessObjectInteraction (SLGObjectType InObjectType)
    {
        if (bShowWnd)
        {
            return;
        }
        switch(InObjectType)
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

    public void OnSLGInit ()
    {
        GameObject mainObject = FindObjectOfType<Ch1MainSystemController>().gameObject;
        if (mainObject != null)
        {
            CH1Data = mainObject.GetComponent<CH1CommonData>();
        }
        MoveOnNextProgress();
        _sponSpots.SetActive(true);
        foreach(SLGInteractionObject Object in _cachedObjects)
        {
            Object.gameObject.SetActive(false);
        }
        if (SLGConstructionObject != null)
        { 
            SLGConstructionObject.SetActive(true);
        }

        _constructUI.SetActive(false);
        RefreshCoinText();

        Ch1StageController SC = FindObjectOfType<Ch1StageController>();
        if(SC != null)
        {
            SC.StageChanger.OnStageEnd += OnChangeStageForSLG;
        }
            
        InitMap();
        MoveOnNextProgress();
    }

    private void InitMap ()
    {
        for (int i = 0; i < Mathf.Min(MAX_SPAWN_COUNT, _cachedObjects.Length); i++)
        {
            SpawnRandomObject();
        }
    }

    private void SpawnRandomObject ()
    {
        if(SLGProgressInfo > SLGProgress.EndConstruction)
        {
            return;
        }

        while(true)
        { 
            int RandomNum = (UnityEngine.Random.Range(0, _cachedObjects.Length));
            if(!_cachedObjects[RandomNum]._isActive)
            {
                SLGObjectType NewType = (SLGObjectType) (UnityEngine.Random.Range(0, (int)SLGObjectType.ASSETMAX));
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
            else if(SLGProgressInfo == SLGProgress.Constructing)
            {
                Wnd_AccelerateSection.gameObject.SetActive(true);
                Wnd_CostSection.gameObject.SetActive(false);

                Wnd_CoinCostText.text = CH1Data._coin.ToString() + "/" + NEEDED_COIN_COUNT.ToString();
                RefreshTimeText();
            }

            bShowWnd = true;
        }
    }

    private void RefreshTimeText()
    {
        //시간 색상 표시 수정
        int DurationTimeSec = DateTime.Now.ToLocalTime().Second - SLGConstructionBeginTime;
        int RemainedTimeSec = NEEDED_CONSTRUCTION_TIME_SEC - DurationTimeSec;
        if (RemainedTimeSec > 0)
        {
            int Hour = RemainedTimeSec / (60 * 60);
            RemainedTimeSec -= 60 * 60 * Hour;

            int Min = RemainedTimeSec / (60);
            int Sec = RemainedTimeSec % (60);
            Wnd_TimeText.text =Hour.ToString() +"시간"+Min.ToString()+"분"+Sec.ToString()+"초";
        }
    }

    private void RefreshCoinText()
    {
        if (UI_CoinText != null)
        {
            if (CH1Data != null)
            {
                UI_CoinText.text = CH1Data._coin.ToString();
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
    private void OnClickConstructBtn ()
    {
        //TODO 건설 가능 시 텍스트 색 표시 수정
        if (_wood >= NEEDED_ASSET_COUNT && _stone >= NEEDED_ASSET_COUNT)
        {
            Debug.Log("건설가능");
            MoveOnNextProgress();
        }
        else
        {
            Debug.Log("건설불가능");
        }
    }

    private void OnChangeStageForSLG ()
{
    if (CH1Data != null)
    {
        CH1Data._sceneMoveCount++;
        if (CH1Data._sceneMoveCount >= SCENE_MOVE_COUNT_SPAWN)
        {
            SpawnRandomObject();
            CH1Data._sceneMoveCount = 0;
        }
    }
}

    private void MoveOnNextProgress()
    {
        SLGProgressInfo++;
        Managers.Data.SLGProgressData = SLGProgressInfo;

        if (SLGProgressInfo < SLGProgress.ModeClose)
        {
            if (bShowWnd)
            {
                RefreshConstructionWnd();
            }
            if(SLGProgressInfo == SLGProgress.Constructing)
            {
                Managers.Data.SLGConstructionBeginTime = DateTime.Now.Second;
                SLGConstructionBeginTime = DateTime.Now.Second;
            }
        }
        else
        {
            EndSLGMode();
        }
    }

    private void EndSLGMode()
    {
        //SLG 끝난 처리
    }

    private void OnClickAccelerateBtn()
    {
        GameObject mainObject = FindObjectOfType<Ch1MainSystemController>().gameObject;
        if (mainObject != null)
        {
            CH1CommonData CH1Data = mainObject.GetComponent<CH1CommonData>();
            if (CH1Data != null)
            {
                if (CH1Data._coin >= NEEDED_COIN_COUNT)
                {
                    CH1Data._coin -= NEEDED_COIN_COUNT;
                    RefreshCoinText();
                    MoveOnNextProgress();
                }
                else
                {
                    Debug.Log("건설 불가능");
                }
            }
        }
    }
}
