using UnityEngine;

public static class Boostrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Debug.Log("initialize boostrapper");

        ServiceLocator.Clear();

        GameObject systemsPrefab = Resources.Load<GameObject>("SystemsManager");

        if (systemsPrefab != null)
        {
            Object.Instantiate(systemsPrefab);
            Debug.Log("Systems Manager Instantiated Successfully.");
        }
        else
        {
            Debug.LogError("Failed to load SystemsManager");
        }
    }
}