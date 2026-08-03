using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class PickupBase : MonoBehaviour
{
    [SerializeField] protected PickupData pickupData;
    private AudioManager audioManager;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;

        audioManager = ServiceLocator.Get<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ApplyPickup(other.gameObject))
            {
                Destroy(gameObject);
            }
        }
    }
    protected abstract bool ApplyPickup(GameObject player);
}