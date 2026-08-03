using UnityEngine;

public class AmmoPickup : PickupBase
{
    protected override bool ApplyPickup(GameObject player)
    {
        PlayerShooting shooting = player.GetComponent<PlayerShooting>();
        if (shooting != null)
        {
            shooting.AddAmmo((int)pickupData.amount);
            Debug.Log($"Picked up {pickupData.pickupName}! Gained {pickupData.amount} Ammo.");
            return true;
        }
        return false;
    }
}