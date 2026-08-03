using System;

public interface IGameplayManager
{
    event Action OnLevelStarted;
    event Action OnWinConditionMet;
    event Action OnLoseConditionMet;

    event Action<int, int> OnProgressChanged;
    bool IsPaused { get; set; }
    void SetConfig(LevelConfig levelConfig);
    void StartLevel();

    void RegisterEnemyDeath();
    void RegisterPlayerDeath();
}