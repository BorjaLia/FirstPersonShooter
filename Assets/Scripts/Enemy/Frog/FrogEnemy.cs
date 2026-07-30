using UnityEngine;

public class FrogEnemy : EnemyBase
{
    [Header("Configuration")]
    [SerializeField] private FrogData frogConfig;

    public override EnemyData baseStats => frogConfig;
    public FrogData config => frogConfig;

    public FrogIdleState IdleState { get; private set; }
    public FrogJumpState JumpState { get; private set; }

    protected override void Start()
    {
        base.Start();
        IdleState = new FrogIdleState(this);
        JumpState = new FrogJumpState(this);

        ChangeState(IdleState);
    }

    protected override void Die()
    {
        Explode();
        Destroy(gameObject);
    }

    public void Explode()
    {
        if (config.explosionVFX != null)
            Instantiate(config.explosionVFX, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, config.explosionRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                col.GetComponent<PlayerHealth>().TakeDamage(config.explosionDamage);
                Debug.Log("Player caught in Frog Explosion");
            }
        }
    }
}
public class FrogIdleState : IState
{
    private FrogEnemy frog;

    public FrogIdleState(FrogEnemy frog) { this.frog = frog; }

    public void Enter()
    {
        frog.Agent.isStopped = true;
        frog.Anim.SetTrigger("Idle");
    }

    public void UpdateLogic()
    {
        float distance = Vector3.Distance(frog.transform.position, frog.playerTarget.position);
        if (distance <= frog.config.jumpTriggerDistance)
        {
            frog.ChangeState(frog.JumpState);
        }
    }

    public void Exit() { }
}

public class FrogJumpState : IState
{
    private FrogEnemy frog;
    private float jumpTimer;

    public FrogJumpState(FrogEnemy frog) { this.frog = frog; }

    public void Enter()
    {
        jumpTimer = 0f;
        frog.Anim.SetTrigger("Jump");

        frog.Agent.isStopped = true;
        frog.Agent.updatePosition = false;
    }

    public void UpdateLogic()
    {
        jumpTimer += Time.deltaTime;

        float jumpProgress = jumpTimer / frog.config.jumpDuration;
        Vector3 targetPos = new Vector3(frog.playerTarget.position.x, frog.transform.position.y, frog.playerTarget.position.z);
        frog.transform.position = Vector3.Lerp(frog.transform.position, targetPos, jumpProgress * Time.deltaTime * 5f);

        if (jumpTimer >= frog.config.jumpDuration)
        {
            frog.Explode();
            Object.Destroy(frog.gameObject);
        }
    }
    public void Exit() { }
}