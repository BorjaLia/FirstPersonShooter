using UnityEngine;

public static class Boostrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Debug.Log("initialize boostrapper");

        ServiceLocator.Clear();

        GameSettingsManager settingsManager = new GameSettingsManager();
        ServiceLocator.Register<GameSettingsManager>(settingsManager);

        GameObject systemsPrefab = Resources.Load<GameObject>("SystemsManager");

        if (systemsPrefab != null)
        {
            GameObject system = Object.Instantiate(systemsPrefab);
            Object.DontDestroyOnLoad(system);
            Debug.Log("Systems Manager Instantiated Successfully.");
        }
        else
        {
            Debug.LogError("Failed to load SystemsManager");
        }
    }
}