using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Data.Original
{
    public class ControlsData
    {
        public GameOverControls GameOverControls { get; private set; }
        
        private int _uiStack = 0;
        
        public ControlsData()
        {
            ResetControls();
        }
        
        public void ResetControls()
        {
            if (GameOverControls != null)
            {
                GameOverControls.Disable();
            }
            GameOverControls = new GameOverControls();
            
            _uiStack = 0;
        }

        public void RestrictPlayerInput()
        {
            _uiStack++;
            GameOverControls.Player.Disable();
        }
        
        public void ReleasePlayerInput()
        {
            if (_uiStack <= 0)
            {
                return;
            }
            
            _uiStack--;
            
            if (_uiStack > 0)
            {
                return;
            }

            GameOverControls.Player.Enable();
            _uiStack = 0;
        }
    }
}