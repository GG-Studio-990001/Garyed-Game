using SLGDefines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Runtime.CH1.Main.Stage;
using Runtime.CH1.Main.Controller;

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

    [SerializeField] private TextMeshProUGUI Wnd_WoodText;
    [SerializeField] private TextMeshProUGUI Wnd_StoneText;
    [SerializeField] private TextMeshProUGUI Wnd_TimeText;
    [SerializeField] private Button Wnd_ConstructionBtn;
    [SerializeField] private Button Wnd_CloseBtn;

    [SerializeField] private GameObject _sponSpots;
    [SerializeField] private GameObject SLGConstructionObject;


    private SLGInteractionObject[] _cachedObjects;
    private int _spawnCount = 0;

    //Data 파일로 따로 분리?
    private int _wood = 0;
    private int _stone = 0;
    private DateTime BeginTime;

    //CONST Value 
    const int INCREASE_ASSET_COUNT = 10;
    const int MAX_SPAWN_COUNT = 3;
    const int NEEDED_ASSET_COUNT = 30;
    const int NEEDED_CONSTRUCTION_TIME_SEC = 60 * 60 * 24;
    const int SCENE_MOVE_COUNT_SPAWN = 5;

    public bool bShowWnd;

    void Start()
    {
        _sponSpots.SetActive(false);
        _cachedObjects = _sponSpots.GetComponentsInChildren<SLGInteractionObject>();
        Wnd_ConstructionBtn.onClick.AddListener(OnClickConstructBtn);
        Wnd_CloseBtn.onClick.AddListener(OnClickCloseBtn);
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
                    if(_constructUI)
                    {
                        Wnd_WoodText.text = _wood.ToString() + "/" + NEEDED_ASSET_COUNT.ToString();
                        Wnd_StoneText.text = _stone.ToString() + "/" + NEEDED_ASSET_COUNT.ToString();
                        RefreshTimeText();
                        _constructUI.SetActive(true);
                        bShowWnd = true;
                    }
                    break;
                }
            default: break;
        }
    }

    public void OnSLGInit ()
    {
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
        BeginTime = DateTime.Now.ToLocalTime();

        Ch1StageController SC = FindObjectOfType<Ch1StageController>();
        if(SC != null)
        {
            SC.StageChanger.OnStageEnd += OnChangeStageForSLG;
        }
            
        InitMap();
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

    private void RefreshTimeText()
    {
        //Load Time Data
        int DurationTimeSec = (int)((DateTime.Now.ToLocalTime() - BeginTime).TotalSeconds);
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
        //TODO 건설 가능 시 텍스트 표시 수정
        //TODO 건설 불가 표시 / OR 건설 완료
        if (_wood >= NEEDED_ASSET_COUNT && _stone >= NEEDED_ASSET_COUNT)
        {
            Debug.Log("건설가능");
        }
        else
        {
            Debug.Log("건설불가능");
        }
    }

    private void OnChangeStageForSLG ()
    {
        GameObject mainObject = FindObjectOfType<Ch1MainSystemController>().gameObject;
        if (mainObject != null)
        {
            CH1CommonData CH1Data = mainObject.GetComponent<CH1CommonData>();
            if(CH1Data != null)
            {
                CH1Data._sceneMoveCount++;
                if (CH1Data._sceneMoveCount >= SCENE_MOVE_COUNT_SPAWN)
                {
                    SpawnRandomObject();
                    CH1Data._sceneMoveCount = 0;
                }
            }
        }
    }
}
