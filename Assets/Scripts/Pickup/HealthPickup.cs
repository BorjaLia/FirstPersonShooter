using UnityEngine;

public class HealthPickup : PickupBase
{
    protected override bool ApplyPickup(GameObject player)
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeHealing(pickupData.amount);
            Debug.Log($"Picked up {pickupData.pickupName}! Restored {pickupData.amount} HP.");
            return true;
        }
        return false;
    }
}