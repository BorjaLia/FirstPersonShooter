using UnityEngine;

public class FrogEnemy : Enemy
{
    [Header("Frog Specifics")]
    public float explosionRadius = 5f;
    public float explosionDamage = 50f;
    public float jumpDistance = 10f;
    public float explotionDistance = 10f;
    public GameObject explosionVFX;

    public FrogIdleState IdleState { get; private set; }
    public FrogJumpState JumpState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        IdleState = new FrogIdleState(this);
        JumpState = new FrogJumpState(this);
    }

    private void Start()
    {
        ChangeState(IdleState);
    }

    protected override void Die()
    {
        Explode();
        base.Die();
    }

    public void Explode()
    {
        if (explosionVFX != null) Instantiate(explosionVFX, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                //col.GetComponent<PlayerHealth>().TakeDamage(explosionDamage);
            }
        }
    }
}