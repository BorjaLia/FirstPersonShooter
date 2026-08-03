using System;
using UnityEngine;

public class GameplayLevelManager : MonoBehaviour, IGameplayManager
{
    public event Action OnLevelStarted;
    public event Action OnWinConditionMet;
    public event Action OnLoseConditionMet;

    private LevelConfig currentLevelConfig;

    private int activeEnemyCount = 0;
    private bool isGameOver = false;

    private void Awake()
    {
        ServiceLocator.Register<IGameplayManager>(this);
        Debug.Log("GameplayLevelManager registered to ServiceLocator.");
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<IGameplayManager>();
    }

    public void SetConfig(LevelConfig levelConfig)
    {
        currentLevelConfig = levelConfig;
    }

    public void StartLevel()
    {
        if (currentLevelConfig == null)
        {
            Debug.LogError("Cannot start level");
            return;
        }

        isGameOver = false;
        activeEnemyCount = 0;

        SpawnPlayer();
        SpawnEnemies();

        ServiceLocator.Get<AudioManager>().PlayGameplayMusic();

        OnLevelStarted?.Invoke();
        Debug.Log($"Level Started! Enemies to defeat: {activeEnemyCount}");
    }

    private void SpawnPlayer()
    {
        Debug.Log("SpawnPlayer");
        if (currentLevelConfig.playerPrefab != null)
        {
            Debug.Log("SpawnedPlayer");
            Instantiate(
                currentLevelConfig.playerPrefab,
                currentLevelConfig.playerSpawnPosition,
                Quaternion.identity
            );
        }
    }

    private void SpawnEnemies()
    {
        foreach (var enemyData in currentLevelConfig.enemiesToSpawn)
        {
            if (enemyData.enemyPrefab != null)
            {
                Instantiate(enemyData.enemyPrefab, enemyData.spawnPosition, Quaternion.identity);
                activeEnemyCount++;
            }
        }
    }

    public void RegisterEnemyDeath()
    {
        if (isGameOver) return;

        activeEnemyCount--;
        Debug.Log($"Enemy defeated. Remaining: {activeEnemyCount}");

        if (currentLevelConfig.winOnAllEnemiesDefeated && activeEnemyCount <= 0)
        {
            isGameOver = true;
            OnWinConditionMet?.Invoke();
            Debug.Log("Won");
        }
    }

    public void RegisterPlayerDeath()
    {
        if (isGameOver) return;

        isGameOver = true;
        OnLoseConditionMet?.Invoke();
        Debug.Log("Lost");
    }
}