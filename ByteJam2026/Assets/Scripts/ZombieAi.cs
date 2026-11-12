using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float runSpeed = 3.5f;
    [SerializeField] private float rotationSpeed = 8f;

    [Header("Player Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionDistance = 5f;
    [SerializeField] private float safeDistance = 8f;
    [SerializeField] private float attackDistance = 1.5f;

    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Attack")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;

private float nextAttackTime;

    private Vector3 moveDirection;
    private float stateTimer;
    private UnityEngine.AI.NavMeshAgent agent;

    private void Awake(){
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!agent.isActiveAndEnabled || !agent.isOnNavMesh)
        {
            return;
        }

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance <= attackDistance)
        {
            Attack();
        }
        else
        {
            Chase();
        }
    }

    private void Chase()
    {
        agent.isStopped = false;
        agent.speed = runSpeed;
        agent.SetDestination(player.position);

        animator.SetBool("IsRunning", true);
        animator.SetBool("IsAttacking", false);
    }

    private void Attack()
    {
        agent.isStopped = true;

        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAttacking", true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (animator.GetBool("IsAttacking"))
            {
                if (Time.time >= nextAttackTime)
                {
                    PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(attackDamage);
                        nextAttackTime = Time.time + attackCooldown;
                    }
                }
            }
        }
    }
}