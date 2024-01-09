using System;
using UnityEngine;

namespace Runtime.CH1.SubB
{
    public class ThreeMatchPuzzleController : MonoBehaviour
    {
        public bool IsClear { get; set; }
        
        private void OnEnable()
        {
            if (IsClear)
            {
                return;
            }
            
            Reset();
        }

        private void Reset()
        {
            
        }
        
        public bool ValidateMovement()
        {
            return false;
        }
    }
}
