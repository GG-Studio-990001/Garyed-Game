using Cinemachine;
using System;
using UnityEngine;

namespace Runtime.CH1.Main.Map
{
    public class Stage : MonoBehaviour
    {
        /*
         * 현재 맵에서 설정해야 하는 정보
         * 자신의 스테이지를 가지고 있어야 하고
         * 맵의 카메라 confiner 정보를 가지고 있어야 한다.
         */
        [SerializeField] private MapMoveCollider mapMoveCollider;
        [SerializeField] private PolygonCollider2D mapConfiner;
        [field: SerializeField] public int StageNumber { get; private set; }
        
        public Ch1GameController Ch1GameController { get; set; } // 이후에 가능하다면 Game Controller 인터페이스로 분리
        public CinemachineConfiner2D CinemachineConfiner2D { get; set; }

        private void Start()
        {
            
        }

        public void SetMapSetting()
        {
            mapMoveCollider.Ch1GameController = Ch1GameController;
            CinemachineConfiner2D.m_BoundingShape2D = mapConfiner;
        }
    }
}
