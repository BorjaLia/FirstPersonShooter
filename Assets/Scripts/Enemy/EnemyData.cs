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

    [Header("Base Audio")]
    public AudioClip attackSound;
    public AudioClip hitSound;
}