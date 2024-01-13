using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Data.Original
{
    public class ControlsData
    {
        public GameOverControls GameOverControls { get; set; }
        
        public ControlsData()
        {
            GameOverControls = new GameOverControls();
        }
        
        private int _uiStack = 0;

        public void RestrictPlayerInput()
        {
            _uiStack++;
            GameOverControls.Player.Disable();
        }
        
        public void ReleasePlayerInput()
        {
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