using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private const float maxHealth = 100.0f;

    private float currentHealth = maxHealth;

    public event Action<float, float> OnHealthChanged;

    private void Awake() { ServiceLocator.Register(this); }
    private void OnDestroy() { ServiceLocator.Unregister<PlayerHealth>(); }
    private void Start() { OnHealthChanged?.Invoke(currentHealth, maxHealth); }
    public void TakeDamage(float damage)
    {
        if(damage < 0.0f)
        {
            Debug.LogWarning("Negative damage! heal with the corresponding function");
            TakeHealing(-damage);
            return;
        }

        currentHealth -= damage;

        if(currentHealth <= 0.0f)
        {
            currentHealth = 0.0f;
            Die();
        }

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeHealing(float healing)
    {
        currentHealth += healing;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        Debug.Log("Player died!");
    }
}
