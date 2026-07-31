using UnityEngine;

public class BulletImpact : MonoBehaviour
{
    [SerializeField] private float lifetime = 5.0f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}