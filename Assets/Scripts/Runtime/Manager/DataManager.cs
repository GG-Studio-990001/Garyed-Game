using Runtime.Data.Original;
using Runtime.Input;
using System;
using System.Numerics;
using UnityEngine;

namespace Runtime.Manager
{
    // 게임 데이터 CH별로 따로 클래스 만들어야 할 듯
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
        
        public GameData()
        {
            chapter = 1;
            stage = 1;
            minor = 0;
            IsPacmomPlayed = false;
            IsPacmomCleared = false;
            musicVolume = 1;
            sfxVolume = 1;
        }
    }
    
    public class DataManager
    {
        private GameData _gameData = new();
        private GameData SaveData { get { return _gameData; } set { _gameData = value; } }
        
        public GameOverControls GameOverControls { get; set; }
        public InGameKeyBinder InGameKeyBinder { get; set; }

        #region properties

        public int Chapter { get { return _gameData.chapter; } set { _gameData.chapter = value; } }
        public int Stage { get { return _gameData.stage; } set { _gameData.stage = value; } }
        public int Minor { get { return _gameData.minor; } set { _gameData.minor = value; } }
        public bool IsPacmomPlayed { get { return _gameData.IsPacmomPlayed; } set { _gameData.IsPacmomPlayed = value; } }
        public bool IsPacmomCleared { get { return _gameData.IsPacmomCleared; } set { _gameData.IsPacmomCleared = value; } }

        public float MusicVolume
        {
            get { return _gameData.musicVolume; } 
            set { Math.Clamp(value, 0, 1); _gameData.musicVolume = value; Managers.Sound.BGM.volume = value;}
        }

        public float SfxVolume
        {
            get { return _gameData.sfxVolume; } 
            set { Math.Clamp(value, 0, 1); _gameData.sfxVolume = value; Managers.Sound.Effect.volume = value;}
        }

        #endregion
        
        public void Init()
        {
            GameOverControls = new GameOverControls();
            InGameKeyBinder = new InGameKeyBinder(GameOverControls);
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
        
        public void NewGame()
        {
            _gameData = new GameData();
            SaveGame();
        }

    }
}