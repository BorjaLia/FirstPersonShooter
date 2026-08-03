using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    private void Start()
    {
        var gameplayManager = ServiceLocator.Get<IGameplayManager>();
        gameplayManager.StartLevel();
    }
}