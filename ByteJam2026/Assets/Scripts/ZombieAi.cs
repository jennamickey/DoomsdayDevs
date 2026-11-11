using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    private enum State
    {
        Chase
    }

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float runSpeed = 3.5f;
    [SerializeField] private float rotationSpeed = 8f;

    [Header("Decision Times")]
    [SerializeField] private float minStateTime = 2f;
    [SerializeField] private float maxStateTime = 5f;

    [Header("Player Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionDistance = 5f;
    [SerializeField] private float safeDistance = 8f;

    [Header("References")]
    [SerializeField] private Animator animator;

    private State currentState;
    private Vector3 moveDirection;
    private float stateTimer;

    private void Start()
    {
        ChangeState(State.Chase);
    }

    private void Update()
    {
        float distanceToPlayer =
            Vector3.Distance(transform.position, player.position);

        Chase();
    }

    private void Chase()
    {
        // Direction from the player toward the boar.
        Vector3 chaseDirection =
            player.position - transform.position;

        chaseDirection.y = 0f;
        chaseDirection.Normalize();

        Move(chaseDirection, runSpeed);
    }

    private void ChangeState(State newState)
    {
        // Don't restart the same state every frame.
        if (currentState == newState)
            return;

        currentState = newState;

        UpdateAnimation();
    }

    private void Move(Vector3 direction, float speed)
    {
        transform.position +=
            direction * speed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(
                transform.forward,
                direction,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void UpdateAnimation()
    {

        animator.SetBool(
            "IsRunning",
            currentState == State.Chase
        );
    }
}