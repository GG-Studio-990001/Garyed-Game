using Runtime.Data.Original;
using System;
using System.Numerics;
using UnityEngine;

namespace Runtime.Manager
{
    [Serializable]
    public class GameData
    {
        public int chapter; // enum으로 변경
        public int stage;
        public int minor;
        public bool IsPacmomPlayed;
        public bool IsPacmomCleared;
        public float musicVolume;
        public float sfxVolume;
    }
    
    public class DataManager
    {
        private GameData _gameData = new();
        private GameData SaveData { get { return _gameData; } set { _gameData = value; } }
        
        public GameOverControls GameOverControls { get; set; }

        #region properties

        public int Chapter { get { return _gameData.chapter; } set { _gameData.chapter = value; } }
        public int Stage { get { return _gameData.stage; } set { _gameData.stage = value; } }
        public int Minor { get { return _gameData.minor; } set { _gameData.minor = value; } }
        public bool IsPacmomPlayed { get { return _gameData.IsPacmomPlayed; } set { _gameData.IsPacmomPlayed = value; } }
        public bool IsPacmomCleared { get { return _gameData.IsPacmomCleared; } set { _gameData.IsPacmomCleared = value; } }

        public float MusicVolume
        {
            get { return _gameData.musicVolume; } 
            set { Math.Clamp(value, 0, 1); _gameData.musicVolume = value; }
        }

        public float SfxVolume
        {
            get { return _gameData.sfxVolume; } 
            set { Math.Clamp(value, 0, 1); _gameData.sfxVolume = value; }
        }

        #endregion
        
        public void Init()
        {
            // 시작 데이터 Init
            _gameData.chapter = 1;
            _gameData.stage = 0;
            _gameData.IsPacmomPlayed = false;
            _gameData.IsPacmomCleared = false;
            
            GameOverControls = new GameOverControls();
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
            return true;
        }

    }
}