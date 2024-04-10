using SLGDefines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private GameObject _sponSpots;

    private SLGInteractionObject[] _CachedObjects;
    private int _SpawnCount = 0;

    //Data 파일로 따로 분리?
    private int _wood = 0;
    private int _stone = 0;

    const int INCREASE_ASSET_COUNT = 10;
    const int MAX_SPAWN_COUNT = 3;

    void Start()
    {
        _sponSpots.SetActive(false);
        _CachedObjects = _sponSpots.GetComponentsInChildren<SLGInteractionObject>();
    }

    void Update()
    {
        
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
                    break;
                }
            case SLGObjectType.STONE:
                {
                    _stone += INCREASE_ASSET_COUNT;
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
        foreach(SLGInteractionObject Object in _CachedObjects)
        {
            Object.gameObject.SetActive(false);
        }

        InitMap();
    }

    private void InitMap ()
    {
        for (int i = 0; i < Mathf.Min(MAX_SPAWN_COUNT, _CachedObjects.Length); i++)
        {
            SpawnRandomObject();
        }
    }

    private void SpawnRandomObject ()
    {
        while(true)
        { 
            int RandomNum = (UnityEngine.Random.Range(0, _CachedObjects.Length));
            if(!_CachedObjects[RandomNum]._isActive)
            {
                SLGObjectType NewType = (SLGObjectType) (UnityEngine.Random.Range(0, (int)SLGObjectType.ASSETMAX));
                _CachedObjects[RandomNum].InitInteractionData(NewType);
                _CachedObjects[RandomNum].gameObject.SetActive(true);
                _SpawnCount++;
                break;
            }
        } 
    }
}
