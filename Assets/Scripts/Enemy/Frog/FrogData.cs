
using UnityEngine;

[CreateAssetMenu(fileName = "NewFrogData", menuName = "Enemies/Frog Data")]
public class FrogData : EnemyData
{
    [Header("Frog Specifics")]
    public float explosionRadius = 5.0f;
    public float explosionTimer = 5.0f;
    public float jumpDuration = 1.0f;
    public GameObject explosionVFX;
    [Header("Audio")]
    public AudioClip jumpSound;
}