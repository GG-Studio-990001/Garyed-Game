using UnityEngine;

namespace Runtime.Common.Domain
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerData", order = 0)]
    public class PlayerData : ScriptableObject
    {
        public Quarter quarter;
        public Vector3 position;
    }
}