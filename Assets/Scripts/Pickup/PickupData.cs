using UnityEngine;

[CreateAssetMenu(fileName = "NewPickupData", menuName = "FPS/Pickup Data")]
public class PickupData : ScriptableObject
{
    [Header("Pickup Info")]
    public string pickupName;
    public float amount;
}