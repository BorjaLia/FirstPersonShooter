using System;

public interface IGameplayManager
{
    event Action OnLevelStarted;
    event Action OnWinConditionMet;
    event Action OnLoseConditionMet;

    void StartLevel(LevelConfig levelConfig);

    void RegisterEnemyDeath();
    void RegisterPlayerDeath();
}