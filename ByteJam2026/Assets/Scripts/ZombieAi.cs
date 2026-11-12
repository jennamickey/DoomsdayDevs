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

    [Header("References")]
    [SerializeField] private Animator animator;

    private Vector3 moveDirection;
    private float stateTimer;
    private UnityEngine.AI.NavMeshAgent agent;

    private void Awake(){
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            Debug.LogWarning("Enemy is not on a NavMesh.");
        }
    }
}