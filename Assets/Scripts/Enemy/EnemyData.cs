using UnityEngine;

public abstract class EnemyData : ScriptableObject
{
    [Header("Base Stats")]
    public float maxHealth = 100.0f;

    public float moveSpeed = 3.5f;
    public float acceleration = 8.0f;
    
    public float attackDamage = 20.0f;
    public float detectionRange = 20.0f;
    public float stoppingDistance = 1.0f;
}

[CreateAssetMenu(fileName = "NewMeleeData", menuName = "Enemies/Melee Data")]
public class MeleeData : EnemyData
{
    [Header("Melee Specifics")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
}

[CreateAssetMenu(fileName = "NewRangeData", menuName = "Enemies/Range Data")]
public class RangeData : EnemyData
{
    [Header("Range Specifics")]
    public float attackRange = 10f;
    public float retreatDistance = 4f;
    public float fireRate = 2f;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
}