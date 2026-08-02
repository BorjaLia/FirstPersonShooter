using UnityEngine;

public class FrogEnemy : EnemyBase
{
    [Header("Configuration")]
    [SerializeField] private FrogData frogConfig;

    public override EnemyData baseStats => frogConfig;
    public FrogData config => frogConfig;

    public FrogIdleState IdleState { get; private set; }
    public FrogJumpState JumpState { get; private set; }
    public FrogExplodeState ExplodeState { get; private set; }

    protected override void Start()
    {
        base.Start();
        IdleState = new FrogIdleState(this);
        JumpState = new FrogJumpState(this);
        ExplodeState = new FrogExplodeState(this);

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

        audioManager.PlaySFXOnce(baseStats.attackSound);

        Collider[] colliders = Physics.OverlapSphere(transform.position, config.explosionRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                col.GetComponent<PlayerHealth>().TakeDamage(config.attackDamage);
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
        Debug.Log("Frog entered Idle!");

        frog.anim.Play("Idle");
        frog.agent.isStopped = true;
    }

    public void UpdateLogic()
    {
        float distance = Vector3.Distance(frog.transform.position, frog.playerTarget.position);

        if (distance <= frog.config.detectionRange)
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
    private Vector3 jumpDir;

    public FrogJumpState(FrogEnemy frog) { this.frog = frog; }

    public void Enter()
    {
        Debug.Log("Frog entered Jump!");

        jumpTimer = 0.0f;

        jumpDir = (frog.transform.forward + frog.transform.up).normalized;

        frog.agent.isStopped = true;
        frog.agent.updatePosition = false;
    }

    public void UpdateLogic()
    {
        float distance = Vector3.Distance(frog.transform.position, frog.playerTarget.position);

        if (distance <= frog.config.stoppingDistance)
        {
            frog.ChangeState(frog.ExplodeState);
        }

        jumpTimer -= Time.deltaTime;

        if (jumpTimer > 0.0f) return;

        frog.anim.Play("Idle");
        frog.anim.Play("Jump");
        frog.audioManager.PlaySFXOnce(frog.config.jumpSound);

        frog.transform.LookAt(frog.playerTarget,Vector3.up);

        jumpDir = (Vector3.forward + Vector3.up).normalized * frog.config.acceleration;

        frog.rb.linearVelocity = Vector3.zero;
        frog.rb.AddRelativeForce(jumpDir,ForceMode.VelocityChange);

        jumpTimer = frog.config.jumpDuration;
    }
    public void Exit() { }
}

public class FrogExplodeState : IState
{
    private FrogEnemy frog;
    private float explosionCountown;

    public FrogExplodeState(FrogEnemy frog) { this.frog = frog; }

    public void Enter()
    {
        Debug.Log("Frog entered Explode!");
        frog.anim.Play("Idle");
        explosionCountown = frog.config.explosionTimer;

        frog.rb.freezeRotation = false;

        frog.agent.isStopped = true;
        frog.agent.updatePosition = false;
    }

    public void UpdateLogic()
    {
        explosionCountown -= Time.deltaTime;

        if (explosionCountown > 0.0f) return;

        frog.Explode();
        Object.Destroy(frog.gameObject);
    }
    public void Exit() { }
}