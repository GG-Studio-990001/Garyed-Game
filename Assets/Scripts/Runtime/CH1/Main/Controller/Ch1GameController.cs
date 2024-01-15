using Cinemachine;
using Runtime.InGameSystem;
using UnityEngine;

namespace Runtime.CH1.Main.Controller
{
    public class Ch1GameController : MonoBehaviour
    {
        [Header("System")]
        [SerializeField] private SoundSystem soundSystem;
        [SerializeField] private StageController stageController;
        [SerializeField] private FadeController fadeController;
        
        [Header("Player")]
        [SerializeField] private GameObject player;
        
        [Header("Camera")]
        [SerializeField] private CinemachineConfiner2D cinemachineConfiner2D;
        
        [Header("Cursor")]
        [SerializeField] private Texture2D cursorTexture;
        
        private void Awake()
        {
            stageController.Init(player, cinemachineConfiner2D, fadeController);
            
            //Test
            InitGame();
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
        
        private void InitGame()
        {
            SetMusic("Start");
        }
        
        private void SetMusic(string soundName)
        {
            soundSystem.StopMusic();
            soundSystem.PlayMusic(soundName);
        }
    }
}