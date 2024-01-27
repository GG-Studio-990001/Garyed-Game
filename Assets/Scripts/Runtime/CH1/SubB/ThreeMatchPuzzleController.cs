using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.CH1.SubB
{
    public class ThreeMatchPuzzleController : MonoBehaviour
    {
        public UnityEvent onClear;
        public bool IsClear { get; private set; }
        [field:SerializeField] public List<Jewelry> Jewelries { get; set; }
        
        private ThreeMatchPuzzleLogic _logic;
        
        private void Start()
        {
            _logic = new ThreeMatchPuzzleLogic(Jewelries);
            _logic.IsClear += () =>
            {
                IsClear = true;
                onClear?.Invoke();
            };
            
            foreach (var jewelry in Jewelries)
            {
                jewelry.Controller = this;
            }
        }

        public void PuzzleReset()
        {
            foreach (var jewelry in Jewelries)
            {
                jewelry.ResetPosition();
            }
        }

        public bool ValidateMovement(Jewelry jewelry, Vector2 direction) => _logic.ValidateMovement(jewelry, direction);
        public void CheckMatching() => _logic.CheckMatching();
    }
}
