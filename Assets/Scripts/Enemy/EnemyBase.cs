using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public abstract class EnemyBase : MonoBehaviour
{
    public abstract EnemyData baseStats { get; }

    [Header("Base State")]
    protected float currentHealth;
    public Transform playerTarget;

    public NavMeshAgent Agent { get; private set; }
    public Animator Anim { get; private set; }

    protected IState currentState;

    protected virtual void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();

        if (playerTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTarget = player.transform;
        }
    }

    protected virtual void Start()
    {
        currentHealth = baseStats.maxHealth;
        Agent.speed = baseStats.moveSpeed;
        Agent.acceleration = baseStats.acceleration;
        Agent.stoppingDistance = baseStats.stoppingDistance;
    }

    protected virtual void Update()
    {
        if (playerTarget == null) return;

        currentState?.UpdateLogic();
    }

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Anim.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Anim.SetTrigger("Die");
        Agent.isStopped = true;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 3f);
    }
}