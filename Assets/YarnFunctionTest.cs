using Runtime.InGameSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnFunctionTest : MonoBehaviour
{
    [YarnFunction("add_numbers")]
    public static int AddNumbers(int first, int second)
    {
        return DataProviderManager.Instance.PlayerDataProvider.Get().quarter.minor;
    }
}
