using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerShooting : MonoBehaviour
{
    [Header("Input map")]
    [SerializeField] public InputActionReference shootActionReference;
    [SerializeField] public InputActionReference reloadActionReference;

    [Header("Weapon Stats")]
    [SerializeField] private int magCapacity = 12;
    [SerializeField] private float fireRate = 3.0f;
    [SerializeField] private float reloadTime = 1.5f;

    [Header("Bullet")]
    [SerializeField] private LayerMask bulletLayerMask;
    [SerializeField] private float bulletDamage = 10.0f;
    [SerializeField] private GameObject bulletSFX;
    [SerializeField] private Transform bullets;

    [Header("Audio")]
    [SerializeField] private AudioClip gunShotSound;
    [SerializeField] private AudioClip reloadSound;

    [Header("Ammo Reserves")]
    [SerializeField] private int totalAmmoReserves = 120;
    public event Action<int, int, int> OnAmmoChanged;


    [SerializeField] private Camera cam;

    private AudioManager audioManager;

    private float timeBetweenShots = 1.0f;

    private bool queueShot = false;
    private float lastShotTimer = 0.0f;

    private bool queueReload = false;
    private float currentReloadTime = 0.0f;

    private int currentMag;

    private IGameplayManager gameplayManager;

    private void Awake() { ServiceLocator.Register(this); }
    private void OnDestroy() { ServiceLocator.Unregister<PlayerShooting>(); }

    private void Start()
    {
        audioManager = ServiceLocator.Get<AudioManager>();

        timeBetweenShots = 1.0f / fireRate;
        currentMag = magCapacity;

        gameplayManager = ServiceLocator.Get<IGameplayManager>();

        OnAmmoChanged?.Invoke(currentMag, magCapacity, totalAmmoReserves);
    }


    private void Update()
    {
        if (gameplayManager != null && gameplayManager.IsPaused) return;

        if (shootActionReference.action.IsPressed())
        {
            queueShot = true;
        }

        if (reloadActionReference.action.WasPressedThisFrame())
        {
            queueReload = true;
        }

        ShootUpdate();

        ReloadUpdate();
    }

    void FixedUpdate()
    {
        if (gameplayManager != null && gameplayManager.IsPaused) return;

        Shoot();

        Reload();
    }

    private void Shoot()
    {
        if (!queueShot) return;
        queueShot = false;

        if (!(lastShotTimer == 0.0f)) return;

        if (!(currentReloadTime == 0.0f)) return;

        if (currentMag == 0)
        {
            Debug.Log("No more bulletes");
            return;
        }

        lastShotTimer = timeBetweenShots;

        currentMag--;
        OnAmmoChanged?.Invoke(currentMag, magCapacity, totalAmmoReserves);

        Debug.Log($"Mag: {currentMag} / {magCapacity}");

        audioManager.PlaySFXOnce(gunShotSound);

        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, bulletLayerMask))
        {
            Debug.DrawRay(cam.transform.position, cam.transform.forward * hit.distance, Color.green,15.0f);
            Debug.Log("Did Hit");

            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.gameObject.GetComponent<EnemyBase>().TakeDamage(bulletDamage);
            }
            else
            {
                Instantiate(bulletSFX, hit.point, Quaternion.LookRotation(-hit.normal));
            }
        }
        else
        {
            Debug.DrawRay(cam.transform.position, cam.transform.forward * 1000, Color.red, 15.0f);
            Debug.Log("Did not Hit");
        }
    }
    private void ShootUpdate()
    {
        if (lastShotTimer > 0.0f)
        {
            lastShotTimer -= Time.deltaTime;
            if (lastShotTimer <= 0.0f)
            {
                lastShotTimer = 0.0f;
            }
        }
    }

    private void Reload()
    {
        if (currentMag == magCapacity) return;

        if (!queueReload) return;
        queueReload = false;

        if (!(currentReloadTime == 0.0f)) return;

        currentReloadTime = reloadTime;
        audioManager.PlaySFXOnce(reloadSound);
    }
    private void ReloadUpdate()
    {
        if (currentReloadTime > 0.0f)
        {
            currentReloadTime -= Time.deltaTime;
            if (currentReloadTime <= 0.0f)
            {
                currentReloadTime = 0.0f;

                int bulletsNeeded = magCapacity - currentMag;
                int bulletsToLoad = Mathf.Min(bulletsNeeded, totalAmmoReserves);

                currentMag += bulletsToLoad;
                totalAmmoReserves -= bulletsToLoad;

                OnAmmoChanged?.Invoke(currentMag, magCapacity, totalAmmoReserves);
            }
        }
    }
}
