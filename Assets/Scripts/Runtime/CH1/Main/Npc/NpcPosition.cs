using UnityEngine;

namespace Runtime.CH1.Main.Npc
{
    [System.Serializable]
    public class LocationArray
    {
        public Vector3[] Locations;
    }

    // 세이브 데이터 불러올 때 달러, 파머, 알투몬 위치
    public class NpcPosition : MonoBehaviour
    {
        [SerializeField] private GameObject[] _npcs;
        public LocationArray[] NpcLocations;
        // 0:초기 1:씬1 이후 2:씬2 시작 3:씬2 이동자리
        // 4:동굴 출구 앞(다리건넌 후) 5:동굴에서 나온 후 (알투몬 조정)
        // 6: 맵3으로 이동하는 1차 위치 7: 맵3의 기본 위치

        // 0:초기 1:씬1.1(추가) 2:씬1 이후 3:씬2 시작 4:씬2 이동자리
        // 5:동굴 출구 앞(다리건넌 후) 6:동굴에서 나온 후 (알투몬 조정)
        // 7: 맵3으로 이동하는 1차 위치 8: 맵3의 기본 위치

        public void LoadNpcPosition()
        {
            Debug.Log("NpcPosition:");
            if (Managers.Data.Scene == 0)
            {
                SetNpcPosition(0);
                Debug.Log("초기 세팅");
            }
            else if (Managers.Data.Scene == 1)
            {
                SetNpcPosition(2);
                Debug.Log("씬1 후");
            }
            else if (Managers.Data.Scene == 2)
            {
                SetNpcPosition(4);
                Debug.Log("씬2 후 이동");
            }
            else if (Managers.Data.Scene == 3)
            {
                SetNpcPosition(6);
                Debug.Log("동굴 출구 앞");
            }
            else // if (Managers.Data.Scene == 4)
            {
                SetNpcPosition(8);
                Debug.Log("맵3");
            }
        }

        public void SetNpcPosition(int idx)
        {
            for (int i=0; i<_npcs.Length; i++)
            {
                _npcs[i].transform.position = NpcLocations[i].Locations[idx];
            }
        }

        public void ActiveNpcs(bool active)
        {
            for (int i = 0; i < _npcs.Length; i++)
            {
                _npcs[i].SetActive(active);
            }
        }
    }
}