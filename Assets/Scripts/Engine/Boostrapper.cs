using UnityEngine;

public static class Boostrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Debug.Log("initialize boostrapper");

        ServiceLocator.Clear();
    }
}