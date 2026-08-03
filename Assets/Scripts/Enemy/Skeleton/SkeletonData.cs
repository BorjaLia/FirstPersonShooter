using UnityEngine;

[CreateAssetMenu(fileName = "NewSkeletonData", menuName = "Enemies/Skeleton Data")]
public class SkeletonData : EnemyData
{
    [Header("Skeleton Specifics")]
    public float attackCooldown = 1.2f;
}