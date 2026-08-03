using UnityEngine;

[CreateAssetMenu(fileName = "FlyingEyeData", menuName = "Enemies/Flying Eye Data")]
public class FlyingEyeData : EnemyData
{
    [Header("Flying Eye Settings")]
    public float fleeRange = 15.0f;
    public float fleeDistance = 8.0f;
    public float attackRange = 5.0f;
    public float attackCooldown = 2.0f;

    [Header("Attack Projectile")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 20.0f;
}