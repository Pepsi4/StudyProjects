using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIHumanoid : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float walkPointRange;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject weapon;

    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private TMPro.TextMeshProUGUI stateDebugText;
    private StateMachine _stateMachine;

    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    private Attack attack;
    private const int ATTACK_OUT_OF_RANGE_DELTA = 4; // used as range delta for attack

    private void Awake()
    {
        _stateMachine = new StateMachine();
        var patrolling = new Patrolling(whatIsGround, agent, walkPointRange, transform, animator, stateDebugText);
        var chase = new Chase(agent, animator, transform, target, stateDebugText);
        attack = new Attack(animator, transform, target, bullet, this, weapon, stateDebugText);

        At(patrolling, chase, InChaseRange());
        At(chase, attack, InAttackRange());
        At(attack, chase, IsOutAttackRange());
        At(chase, patrolling, IsOutOfRange());

        _stateMachine.SetState(patrolling);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        Func<bool> IsOutOfRange() => () => Physics.CheckSphere(transform.position, sightRange, whatIsPlayer) == false;
        Func<bool> IsOutAttackRange() => () => Physics.CheckSphere(transform.position, attackRange + ATTACK_OUT_OF_RANGE_DELTA, whatIsPlayer) == false;
        Func<bool> InChaseRange() => () => Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        Func<bool> InAttackRange() => () => Physics.CheckSphere(transform.position, attackRange, whatIsPlayer) && CheckIfPlayerRayIsValid() == true;
    }

    private void Update() => _stateMachine.Tick();

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = UnityEngine.Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }

    public void Shoot()
    {
        attack.AttackTarget();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private bool CheckIfPlayerRayIsValid()
    {
        Vector3 deltaCheck = new Vector3(0.2f, 0, 0);
        Ray rayLeft = new Ray(transform.position + Vector3.up + deltaCheck, (target.transform.position - transform.position));
        Ray rayRight = new Ray(transform.position + Vector3.up - deltaCheck, (target.transform.position - transform.position));

        RaycastHit hitDataLeft, hitDataRight;
        Debug.DrawRay(transform.position + Vector3.up + deltaCheck, (target.transform.position - transform.position), Color.green, 10);
        Debug.DrawRay(transform.position + Vector3.up - deltaCheck, (target.transform.position - transform.position), Color.blue, 10);
        if (Physics.Raycast(rayLeft, out hitDataLeft) && Physics.Raycast(rayRight, out hitDataRight))
        {
            if (hitDataLeft.collider.TryGetComponent(out StarterAssets.ThirdPersonController player) && hitDataRight.collider.TryGetComponent(out StarterAssets.ThirdPersonController player2))
            {
                return true;
            }
        }

        return false;
    }
}
