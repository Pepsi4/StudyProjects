using UnityEngine;
using UnityEngine.AI;

public class Patrolling : IState
{
    private LayerMask whatIsGround;
    private Vector3 walkPoint;
    private NavMeshAgent agent;
    private bool walkPointSet;
    private float walkPointRange;
    private Transform transform;
    private Animator animator;
    private TMPro.TextMeshProUGUI stateDebugText;

    private const float SPEED = 1.5f;
    private const float MOTION_SPEED = 3f;

    public Patrolling(LayerMask whatIsGround, NavMeshAgent agent, float walkPointRange, Transform transform, Animator animator, TMPro.TextMeshProUGUI stateDebugText)
    {
        this.whatIsGround = whatIsGround;
        this.agent = agent;
        this.walkPointRange = walkPointRange;
        this.transform = transform;
        this.animator = animator;
        this.stateDebugText = stateDebugText;
    }

    public void OnEnter()
    {
        Debug.Log("Patrolling");
        stateDebugText.text = "Patrolling";
        animator.SetFloat("Speed", SPEED);
        animator.SetFloat("MotionSpeed", MOTION_SPEED);
    }

    public void OnExit() { }

    public void Tick()
    {
        Patroll();
    }

    private void Patroll()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
}
