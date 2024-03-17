using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Manager
{
    // 리소스 매니저로 현재는 Resource로 대체되어 있지만
    // 어드레서블로 교체해야 함
    // 이 부분에서만 교체하면 다 바뀌게 할 수 있게 설정함
    public class ResourceManager
    {
        public Dictionary<string, Sprite> _sprites = new();
        
        public void Init()
        {
        }
        
        public T Load<T>(string)
    }
}