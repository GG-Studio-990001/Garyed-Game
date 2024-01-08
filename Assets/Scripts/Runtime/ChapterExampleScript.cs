using Runtime.Data.Original;
using Runtime.Data.Provider;
using Runtime.InGameSystem;
using Runtime.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterExampleScript : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _textMeshProUGUI;

    private void Start()
    {
        _textMeshProUGUI = GetComponent<TMPro.TextMeshProUGUI>();

        IProvider<PlayerData> playerDataProvider = DataProviderManager.Instance.PlayerDataProvider;
        PlayerData playerData = playerDataProvider.Get();
        
        _textMeshProUGUI.text = $"Chapter: {playerData.quarter.chapter} - {playerData.quarter.stage} - {playerData.quarter.minor}";
    }
}
