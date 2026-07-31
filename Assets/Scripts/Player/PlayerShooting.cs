using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [SerializeField] private Camera cam;

    private float timeBetweenShots = 1.0f;

    private bool queueShot = false;
    private float lastShotTimer = 0.0f;

    private bool queueReload = false;
    private float currentReloadTime = 0.0f;

    private int currentMag;

    private void Start()
    {
        timeBetweenShots = 1.0f / fireRate;
        currentMag = magCapacity;
    }

    private void Update()
    {
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

        Debug.Log($"Mag: {currentMag} / {magCapacity}");

        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, cam.farClipPlane, bulletLayerMask))
        {
            Instantiate(bulletSFX);
            Debug.DrawRay(cam.transform.position, cam.transform.forward * hit.distance, Color.green);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(cam.transform.position, cam.transform.forward * 1000, Color.red);
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
        if (!queueReload) return;
        queueReload = false;

        if (!(currentReloadTime == 0.0f)) return;

        currentReloadTime = reloadTime;
    }
    private void ReloadUpdate()
    {
        if (currentReloadTime > 0.0f)
        {
            currentReloadTime -= Time.deltaTime;
            if (currentReloadTime <= 0.0f)
            {
                currentReloadTime = 0.0f;
                currentMag = magCapacity;
            }
        }
    }
}
