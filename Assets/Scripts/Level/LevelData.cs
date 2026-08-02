using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelConfig", menuName = "Level/Level Config")]
public class LevelConfig : ScriptableObject
{
    [Header("Player Settings")]
    public GameObject playerPrefab;
    public Vector3 playerSpawnPosition;

    [Header("Enemy Settings")]
    public List<EnemySpawnData> enemiesToSpawn;

    [Header("Win Conditions")]
    public bool winOnAllEnemiesDefeated = true;
}

[Serializable]
public struct EnemySpawnData
{
    public GameObject enemyPrefab;
    public Vector3 spawnPosition;
}