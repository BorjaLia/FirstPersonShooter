using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public abstract class Enemy : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField]private const float maxHealth = 100.0f;

    [Header("References")]
    public Transform playerTarget;

    public NavMeshAgent nav { get; private set; }
    public Animator animator { get; private set; }

    protected IState currentState;
    
    protected float currentHealth = maxHealth;

    protected virtual void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        if (!nav) Debug.LogError("No Nav Agent found!");

        animator = GetComponent<Animator>();
        if (!animator) Debug.LogError("No Animator found!");

        currentHealth = maxHealth;

        if (playerTarget == null) playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
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
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}