using Runtime.ETC;
using Runtime.InGameSystem;
using Runtime.Manager;
using UnityEngine;

// 싱글톤 패턴을 사용하여 각종 매니저들을 관리하는 클래스
// 테스트 코드에 적합하지 않지만, 게임 로직과 불필요한 의존성을 줄이기 위해 사용 (사운드, 게임 데이터 등)
// 따라서 다음과 같은 기준을 가짐
// Manager 클래스는 다른 Manager 클래스를 관리하는 클래스로 둠
// 그 아래로 접두사+Manager 클래스로 합성하여 구성한다.
// 데이터는 불변을 보장하고, 떼어내야 하는 로직 (전역적인 로직, 사운드, 기타 등)을 관리
public class Managers : MonoBehaviour
{
    private static Managers _instance;
    public static Managers Instance => _instance;
    
    private static SoundManager _soundManager = new SoundManager();
    private static ResourceManager _resourceManager = new ResourceManager();
    private static DataManager _dataManager = new DataManager();

    public static SoundManager Sound { get { Init(); return _soundManager; } }
    public static ResourceManager Resource { get { Init(); return _resourceManager; } }
    public static DataManager Data { get { Init(); return _dataManager; } }

    public void Start()
    {
        Init();
    }

    public static void Init()
    {
        if (Instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
            }

            _instance = Utils.GetOrAddComponent<Managers>(go);
            DontDestroyOnLoad(go);
            
            // Mangner Init
            _soundManager.Init();
            _resourceManager.Init();
            _dataManager.Init();
        }
    }
}
