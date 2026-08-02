using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelConfig", menuName = "FPS/Level Config")]
public class LevelConfigSO : ScriptableObject
{
    [Header("Player Settings")]
    public GameObject playerPrefab;
    public Transform playerSpawnTransform;

    [Header("Enemy Settings")]
    public List<EnemySpawnData> enemiesToSpawn;

    [Header("Win Conditions")]
    public bool winOnAllEnemiesDefeated = true;
}

[Serializable]
public struct EnemySpawnData
{
    public GameObject enemyPrefab;
    public Transform spawnTransform;
}