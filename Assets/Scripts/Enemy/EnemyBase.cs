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

    public AudioManager audioManager;

    protected IState currentState;

    protected bool isAlive;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
        rb.isKinematic = true;

        isAlive = true;

        if (playerTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTarget = player.transform;
            else Debug.LogError("No player found!");
        }
    }

    protected virtual void Start()
    {
        audioManager = ServiceLocator.Get<AudioManager>();

        currentHealth = baseStats.maxHealth;
        agent.speed = baseStats.moveSpeed;
        agent.acceleration = baseStats.acceleration;
        agent.stoppingDistance = baseStats.stoppingDistance;
    }

    protected virtual void Update()
    {
        if (!isAlive) return;

        if (playerTarget == null) return;

        currentState?.UpdateLogic();
    }

    public void ChangeState(IState newState)
    {
        if (!isAlive) return;

        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public virtual void TakeDamage(float amount)
    {
        if (!isAlive) return;

        currentHealth -= amount;

        Debug.Log($"Enemy: Yeouch! {currentHealth}");

        audioManager.PlaySFXOnce(baseStats.hitSound);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if (!isAlive) return;
        isAlive = false;

        anim.Play("Die");

        agent.isStopped = true;
        agent.updatePosition = false;

        Debug.Log("Enemy died");

        rb.freezeRotation = false;
        rb.isKinematic = false;

        ServiceLocator.Get<IGameplayManager>().RegisterEnemyDeath();

        Destroy(gameObject, 3f);
    }
}