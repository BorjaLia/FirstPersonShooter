using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Health UI")]
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Ammo UI")]
    [SerializeField] private TextMeshProUGUI ammoText;

    [Header("Level Progress UI")]
    [SerializeField] private TextMeshProUGUI progressText;

    private PlayerHealth playerHealth;
    private PlayerShooting playerShooting;
    private IGameplayManager gameplayManager;

    private void Start()
    {
        gameplayManager = ServiceLocator.Get<IGameplayManager>();

        gameplayManager.OnLevelStarted += BindToPlayer;
        gameplayManager.OnProgressChanged += UpdateProgressUI;
    }

    private void OnDestroy()
    {
        if (gameplayManager != null)
        {
            gameplayManager.OnLevelStarted -= BindToPlayer;
            gameplayManager.OnProgressChanged -= UpdateProgressUI;
        }

        if (playerHealth != null) playerHealth.OnHealthChanged -= UpdateHealthUI;
        if (playerShooting != null) playerShooting.OnAmmoChanged -= UpdateAmmoUI;
    }

    private void BindToPlayer()
    {
        playerHealth = ServiceLocator.Get<PlayerHealth>();
        playerShooting = ServiceLocator.Get<PlayerShooting>();

        playerHealth.OnHealthChanged += UpdateHealthUI;
        playerShooting.OnAmmoChanged += UpdateAmmoUI;
    }

    private void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = $"Hp: {currentHealth} / {maxHealth}";
        }
    }

    private void UpdateAmmoUI(int currentMag, int magCapacity, int totalReserves)
    {
        if (ammoText != null)
        {
            ammoText.text = $"{currentMag} / {magCapacity} | {totalReserves}";
        }
    }

    private void UpdateProgressUI(int enemiesDefeated, int totalEnemies)
    {
        if (progressText != null)
        {
            progressText.text = $"Enemies: {enemiesDefeated} / {totalEnemies}";
        }
    }
}