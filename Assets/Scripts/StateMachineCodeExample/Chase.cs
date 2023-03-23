using UnityEngine;
using UnityEngine.AI;

public class Chase : IState
{
    private Animator animator;

    private const float SPEED = 3f;
    private const float MOTION_SPEED = 1.3f;
    private NavMeshAgent agent;
    private Transform transform, target;
    private TMPro.TextMeshProUGUI stateDebugText;

    public Chase(NavMeshAgent agent, Animator animator, Transform transform, Transform target, TMPro.TextMeshProUGUI stateDebugText)
    {
        this.agent = agent;
        this.animator = animator;
        this.transform = transform;
        this.target = target;
        this.stateDebugText = stateDebugText;
    }

    public void OnEnter()
    {
        Debug.Log("Chase");
        stateDebugText.text = "Chase";
        animator.SetFloat("Speed", SPEED);
        animator.SetFloat("MotionSpeed", MOTION_SPEED);
    }

    public void OnExit()
    {
        agent.SetDestination(transform.position);
    }

    public void Tick()
    {
        agent.SetDestination(target.position);
        transform.LookAt(agent.destination);
    }
}
