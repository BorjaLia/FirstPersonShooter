using UnityEngine;

public class EyeBullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 5.0f;

    private float currentLifetime = 0.0f;

    private void Start()
    {
        Debug.Log("Eye bullet shot");
    }

    private void Update()
    {
        currentLifetime += Time.deltaTime;
        if(currentLifetime >= lifetime) Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.GetComponent<PlayerHealth>()) return;
        collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(collision.relativeVelocity.magnitude);
    }
}