
using UnityEngine;

[CreateAssetMenu(fileName = "NewFrogData", menuName = "Enemies/Frog Data")]
public class FrogData : EnemyData
{
    [Header("Frog Specifics")]
    public float explosionRadius = 5f;
    public float explosionDamage = 50f;
    public float jumpTriggerDistance = 8f;
    public float jumpDuration = 1f;
    public GameObject explosionVFX;
}