using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(Rigidbody))]
public abstract class EnemyBase : MonoBehaviour
{
    public abstract EnemyData baseStats { get; }

    [Header("Base State")]
    protected float currentHealth;
    public Transform playerTarget;

    public NavMeshAgent agent { get; private set; }
    public Animator anim { get; private set; }
    public Rigidbody rb { get; private set; }

    protected IState currentState;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        if (playerTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTarget = player.transform;
            else Debug.LogError("No player found!");
        }
    }

    protected virtual void Start()
    {
        currentHealth = baseStats.maxHealth;
        agent.speed = baseStats.moveSpeed;
        agent.acceleration = baseStats.acceleration;
        agent.stoppingDistance = baseStats.stoppingDistance;
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
        anim.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        anim.SetTrigger("Die");
        agent.isStopped = true;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 3f);
    }
}