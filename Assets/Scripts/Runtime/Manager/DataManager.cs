using Runtime.Input;
using System;
using Runtime.ETC;
using UnityEngine;
using Runtime.CH1.Main.Stage;

namespace Runtime.Manager
{
    // 게임 데이터 CH별로 따로 클래스 만들어야 할 듯
    [Serializable]
    public class GameData
    {
        // Progress
        public int Chapter;
        public int Stage;
        public int Scene;
        public int SceneDetail; // 씬 세부 진행도
        // Sound
        public float BgmVolume;
        public float SfxVolume;
        // Pacmom
        public bool IsPacmomPlayed;
        public bool IsPacmomCleared;
        public int PacmomCoin;
        // SLG
        public long SLGConstructionBeginTime;
        public SLGProgress SLGProgressData;
        public int SLGWoodCount;
        public int SLGStoneCount;

        public GameData()
        {
            // Progress
            Chapter = 1;
            Stage = 1;
            Scene = 0;
            SceneDetail = 0;
            // Sound
            BgmVolume = 0.5f;
            SfxVolume = 0.5f;
            // Pacmom
            IsPacmomPlayed = false;
            IsPacmomCleared = false;
            PacmomCoin = 0;
            // SLG
            SLGConstructionBeginTime = 0;
            SLGProgressData = SLGProgress.None;
            SLGWoodCount = 0;
            SLGStoneCount = 0;
        }
    }
    
    public class DataManager
    {
        private GameData SaveData { get { return _gameData; } set { _gameData = value; } }
        public InGameKeyBinder InGameKeyBinder { get; set; }
        
        private GameData _gameData = new();
        private GameOverControls _gameOverControls;

        #region properties
        public int Chapter { get { return _gameData.Chapter; } set { _gameData.Chapter = value; } }
        public int Stage { get { return _gameData.Stage; } set { _gameData.Stage = value; } }
        public int Scene { get { return _gameData.Scene; } set { _gameData.Scene = value; } }
        public int SceneDetail { get { return _gameData.SceneDetail; } set { _gameData.SceneDetail = value; } }

        public float BgmVolume
        {
            get { return _gameData.BgmVolume; }
            set { Math.Clamp(value, 0, 1); _gameData.BgmVolume = value; Managers.Sound.BGM.volume = value; }
        }
        public float SfxVolume
        {
            get { return _gameData.SfxVolume; }
            set { Math.Clamp(value, 0, 1); _gameData.SfxVolume = value; Managers.Sound.SFX.volume = value; }
        }

        // Pacmom
        public bool IsPacmomPlayed { get { return _gameData.IsPacmomPlayed; } set { _gameData.IsPacmomPlayed = value; } }
        public bool IsPacmomCleared { get { return _gameData.IsPacmomCleared; } set { _gameData.IsPacmomCleared = value; } }
        public int PacmomCoin { get { return _gameData.PacmomCoin; } set { _gameData.PacmomCoin = value; } }

        // SLG
        public long SLGConstructionBeginTime { get { return _gameData.SLGConstructionBeginTime; } set { _gameData.SLGConstructionBeginTime = value; } }
        public SLGProgress SLGProgressData { get { return _gameData.SLGProgressData; } set { _gameData.SLGProgressData = value; } }

        public int SLGWoodCount { get { return _gameData.SLGWoodCount; } set { _gameData.SLGWoodCount = value; } }

        public int SLGStoneCount { get { return _gameData.SLGStoneCount; } set { _gameData.SLGStoneCount = value; } }
        #endregion

        public void Init()
        {
            _gameOverControls = new GameOverControls();
            InGameKeyBinder = new InGameKeyBinder(_gameOverControls);
        }
        
        public void SaveGame()
        {
            string _path = Application.persistentDataPath + "/SaveData.json";
            
            string jsonStr = JsonUtility.ToJson(Managers.Data.SaveData);
            System.IO.File.WriteAllText(_path, jsonStr);
            Debug.Log($"Save Game Completed : {_path}");
        }
        
        public bool LoadGame()
        {
            string _path = Application.persistentDataPath + "/SaveData.json";
            
            if (System.IO.File.Exists(_path) == false)
            {
                return false;
            }

            string jsonStr = System.IO.File.ReadAllText(_path);
            GameData data = JsonUtility.FromJson<GameData>(jsonStr);
            if (data == null)
            {
                return false;
            }
            
            Managers.Data.SaveData = data;
            Debug.Log($"Load Game Completed : {_path}");
            Debug.Log(Managers.Data.Scene+"."+ Managers.Data.SceneDetail);
            return true;
        }
        
        public void NewGame()
        {
            _gameData = new GameData();
            SaveGame();
        }
    }
}