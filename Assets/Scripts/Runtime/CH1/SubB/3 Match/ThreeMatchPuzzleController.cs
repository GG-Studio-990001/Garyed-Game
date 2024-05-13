using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.CH1.SubB
{
    public class ThreeMatchPuzzleController : MonoBehaviour
    {
        public UnityEvent onClear;
        [field:SerializeField] public bool IsClear { get; private set; }
        [field:SerializeField] public List<Jewelry> Jewelries { get; set; }
        
        private ThreeMatchPuzzleLogic _logic;
        
        private void Start()
        {
            if (Jewelries.Count == 0 || Jewelries == null)
            {
                Jewelries = new List<Jewelry>(FindObjectsOfType<Jewelry>());
            }
            
            _logic = new ThreeMatchPuzzleLogic(Jewelries);
            
            foreach (var jewelry in Jewelries)
            {
                jewelry.Controller = this;
            }
        }

        public void PuzzleReset()
        {
            if (IsClear)
            {
                return;
            }

            foreach (var jewelry in Jewelries)
            {
                jewelry.ResetPosition();
            }
        }
        
        public void PuzzleClear()
        {
            if (IsClear)
            {
                return;
            }

            IsClear = true;
            onClear?.Invoke();
        }

        public bool ValidateMovement(Jewelry jewelry, Vector2 direction) => _logic.ValidateMovement(jewelry, direction);
        public void CheckMatching() => _logic.CheckMatching();
    }
}
