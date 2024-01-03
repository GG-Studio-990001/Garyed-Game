using Runtime.Data.Original;
using Runtime.Interface;
using UnityEngine;

namespace Runtime.Data.Provider
{
    public class PlayerDataProvider : IProvider<PlayerData>
    {
        private readonly PlayerData _playerData;

        public PlayerDataProvider(PlayerData playerData)
        {
            _playerData = playerData;
        }

        public PlayerData Get()
        {
            PlayerData playerData = ScriptableObject.CreateInstance<PlayerData>();
            playerData.quarter = _playerData.quarter.Clone() as Quarter;
            playerData.position = _playerData.position;
            
            return playerData;
        }

        public void Set(PlayerData value)
        {
            _playerData.quarter.chapter = value.quarter.chapter;
            _playerData.quarter.major = value.quarter.major;
            _playerData.quarter.minor = value.quarter.minor;
            _playerData.position = value.position;
        }
    }
}