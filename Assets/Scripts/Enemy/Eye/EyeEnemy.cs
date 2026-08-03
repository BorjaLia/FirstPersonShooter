using UnityEngine;
using UnityEngine.AI;

public class FlyingEyeEnemy : EnemyBase
{
    [Header("Configuration")]
    [SerializeField] private FlyingEyeData eyeConfig;
    [SerializeField] private Transform shootPoint;

    public override EnemyData baseStats => eyeConfig;
    public FlyingEyeData config => eyeConfig;

    public FlyingEyeIdleState IdleState { get; private set; }
    public FlyingEyeFleeState FleeState { get; private set; }
    public FlyingEyeAttackState AttackState { get; private set; }

    protected override void Start()
    {
        base.Start();

        IdleState = new FlyingEyeIdleState(this);
        FleeState = new FlyingEyeFleeState(this);
        AttackState = new FlyingEyeAttackState(this);

        ChangeState(IdleState);
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        if (currentHealth > 0 && currentState != AttackState)
        {
            Debug.Log("Eye took damage! Forcing Attack State!");
            ChangeState(AttackState);
        }
    }

    public void FireProjectile()
    {
        if (config.projectilePrefab != null && shootPoint != null)
        {
            GameObject proj = Instantiate(config.projectilePrefab, shootPoint.position, Quaternion.identity);

            Vector3 aimTarget = playerTarget.position + Vector3.up;
            proj.transform.LookAt(aimTarget);

            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = proj.transform.forward * config.projectileSpeed;
            }
        }

        if (baseStats.attackSound != null)
        {
            audioManager.PlaySFXOnce(baseStats.attackSound);
        }
    }
}

public class FlyingEyeIdleState : IState
{
    private FlyingEyeEnemy eye;

    public FlyingEyeIdleState(FlyingEyeEnemy eye) { this.eye = eye; }

    public void Enter()
    {
        Debug.Log("Eye: Idle");
        eye.anim.Play("Idle");
        eye.agent.isStopped = true;
    }

    public void UpdateLogic()
    {
        float distance = Vector3.Distance(eye.transform.position, eye.playerTarget.position);

        if (distance <= eye.config.attackRange)
        {
            eye.ChangeState(eye.AttackState);
        }
        else if (distance <= eye.config.fleeRange)
        {
            eye.ChangeState(eye.FleeState);
        }
    }

    public void Exit() { }
}

public class FlyingEyeFleeState : IState
{
    private FlyingEyeEnemy eye;
    private float repathTimer;

    public FlyingEyeFleeState(FlyingEyeEnemy eye) { this.eye = eye; }

    public void Enter()
    {
        Debug.Log("Eye: Fleeing!");
        eye.anim.Play("Fly");
        eye.agent.isStopped = false;
        CalculateFleePoint();
    }

    public void UpdateLogic()
    {
        float distance = Vector3.Distance(eye.transform.position, eye.playerTarget.position);

        if (distance <= eye.config.attackRange)
        {
            eye.ChangeState(eye.AttackState);
            return;
        }
        else if (distance > eye.config.fleeRange + 2.0f)
        {
            eye.ChangeState(eye.IdleState);
            return;
        }

        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0.0f)
        {
            CalculateFleePoint();
            repathTimer = 0.5f;
        }
    }

    private void CalculateFleePoint()
    {
        Vector3 dirAwayFromPlayer = (eye.transform.position - eye.playerTarget.position).normalized;

        Vector3 fleePosition = eye.transform.position + (dirAwayFromPlayer * eye.config.fleeDistance);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePosition, out hit, eye.config.fleeDistance, NavMesh.AllAreas))
        {
            eye.agent.SetDestination(hit.position);
        }
    }

    public void Exit() { }
}

public class FlyingEyeAttackState : IState
{
    private FlyingEyeEnemy eye;
    private float attackTimer;

    public FlyingEyeAttackState(FlyingEyeEnemy eye) { this.eye = eye; }

    public void Enter()
    {
        Debug.Log("Eye: Attacking!");
        eye.agent.isStopped = true;
        eye.agent.updateRotation = false;

        attackTimer = 0.0f;
    }

    public void UpdateLogic()
    {
        Vector3 lookDirection = (eye.playerTarget.position - eye.transform.position).normalized;
        lookDirection.y = 0;
        if (lookDirection != Vector3.zero)
        {
            eye.transform.rotation = Quaternion.Slerp(eye.transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 5f);
        }

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0.0f)
        {
            eye.anim.Play("Attack");
            eye.FireProjectile();
            attackTimer = eye.config.attackCooldown;
        }

        float distance = Vector3.Distance(eye.transform.position, eye.playerTarget.position);
        if (distance > eye.config.attackRange * 1.5f)
        {
            eye.agent.SetDestination(eye.playerTarget.position);
        }
    }

    public void Exit()
    {
        eye.agent.updateRotation = true;
    }
}