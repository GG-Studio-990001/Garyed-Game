using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Data.Original
{
    public class ControlsData
    {
        public GameOverControls GameOverControls { get; private set; }
        
        public ControlsData()
        {
            GameOverControls = new GameOverControls();
        }
    }
}