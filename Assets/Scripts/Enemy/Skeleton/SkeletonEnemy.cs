using UnityEngine;

public class SkeletonEnemy : EnemyBase
{
    [Header("Configuration")]
    [SerializeField] private SkeletonData skeletonConfig;

    public override EnemyData baseStats => skeletonConfig;
    public SkeletonData config => skeletonConfig;

    public SkeletonIdleState IdleState { get; private set; }
    public SkeletonChaseState ChaseState { get; private set; }
    public SkeletonAttackState AttackState { get; private set; }

    protected override void Start()
    {
        base.Start();
        
        IdleState = new SkeletonIdleState(this);
        ChaseState = new SkeletonChaseState(this);
        AttackState = new SkeletonAttackState(this);

        ChangeState(IdleState);
    }

    public void DealMeleeDamage()
    {
        if (!isAlive) return;

        float distance = Vector3.Distance(transform.position, playerTarget.position);
        if (distance <= config.stoppingDistance)
        {
            PlayerHealth pHealth = playerTarget.GetComponent<PlayerHealth>();
            if (pHealth != null)
            {
                pHealth.TakeDamage(config.attackDamage);
                if (baseStats.attackSound != null)
                {
                    audioManager.PlaySFXOnce(baseStats.attackSound);
                }
                Debug.Log("Skeleton slashed the player!");
            }
        }
    }
}

public class SkeletonIdleState : IState
{
    private SkeletonEnemy skeleton;

    public SkeletonIdleState(SkeletonEnemy skeleton) { this.skeleton = skeleton; }

    public void Enter()
    {
        skeleton.anim.Play("Idle");
        skeleton.agent.isStopped = true;
    }

    public void UpdateLogic()
    {
        float distance = Vector3.Distance(skeleton.transform.position, skeleton.playerTarget.position);

        if (distance <= skeleton.config.detectionRange)
        {
            skeleton.ChangeState(skeleton.ChaseState);
        }
    }

    public void Exit() { }
}

public class SkeletonChaseState : IState
{
    private SkeletonEnemy skeleton;

    public SkeletonChaseState(SkeletonEnemy skeleton) { this.skeleton = skeleton; }

    public void Enter()
    {
        skeleton.anim.Play("Run");
        skeleton.agent.isStopped = false;
    }

    public void UpdateLogic()
    {
        skeleton.agent.SetDestination(skeleton.playerTarget.position);

        float distance = Vector3.Distance(skeleton.transform.position, skeleton.playerTarget.position);

        if (distance <= skeleton.config.stoppingDistance)
        {
            skeleton.ChangeState(skeleton.AttackState);
        }
        else if (distance > skeleton.config.detectionRange * 1.5f)
        {
            skeleton.ChangeState(skeleton.IdleState);
        }
    }

    public void Exit() { }
}

public class SkeletonAttackState : IState
{
    private SkeletonEnemy skeleton;
    private float attackTimer;

    public SkeletonAttackState(SkeletonEnemy skeleton) { this.skeleton = skeleton; }

    public void Enter()
    {
        skeleton.agent.isStopped = true;
        attackTimer = 0.0f;
    }

    public void UpdateLogic()
    {
        Vector3 lookPos = skeleton.playerTarget.position;
        lookPos.y = skeleton.transform.position.y;
        skeleton.transform.LookAt(lookPos);

        float distance = Vector3.Distance(skeleton.transform.position, skeleton.playerTarget.position);

        if (distance > skeleton.config.stoppingDistance)
        {
            skeleton.ChangeState(skeleton.ChaseState);
            return;
        }

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0.0f)
        {
            skeleton.anim.Play("Attack");
            skeleton.DealMeleeDamage();
            attackTimer = skeleton.config.attackCooldown;
        }
    }

    public void Exit() { }
}