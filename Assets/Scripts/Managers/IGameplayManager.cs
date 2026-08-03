using System;

public interface IGameplayManager
{
    event Action OnLevelStarted;
    event Action OnWinConditionMet;
    event Action OnLoseConditionMet;

    bool IsPaused { get; set; }
    void SetConfig(LevelConfig levelConfig);
    void StartLevel();

    void RegisterEnemyDeath();
    void RegisterPlayerDeath();
}