using SLGDefines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [SerializeField] private GameObject _sponSpots;

    private SLGInteractionObject[] _cachedObjects;
    private int _spawnCount = 0;

    //Data 파일로 따로 분리?
    private int _wood = 0;
    private int _stone = 0;

    const int INCREASE_ASSET_COUNT = 10;
    const int MAX_SPAWN_COUNT = 3;

    void Start()
    {
        _sponSpots.SetActive(false);
        _cachedObjects = _sponSpots.GetComponentsInChildren<SLGInteractionObject>();
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
                        _constructUI.SetActive(true);
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

        InitMap();
    }

    private void RefreshAssetUI()
    {
        
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
}
