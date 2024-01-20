using System.Collections;
using UnityEngine;

public class Attack : IState
{
    private Animator animator;
    private Transform target;
    private GameObject bullet;
    private Transform transform;
    private GameObject weapon;

    private MonoBehaviour mono;
    private bool alreadyAtacked;
    private const float SPEED = 0f;
    private const float MOTION_SPEED = 0f;
    private const float TIMEBETWEENATTACK = 1f;
    private TMPro.TextMeshProUGUI stateDebugText;

    public Attack(Animator animator, Transform transform, Transform target, GameObject bullet, MonoBehaviour mono, GameObject weapon, TMPro.TextMeshProUGUI stateDebugText)
    {
        this.animator = animator;
        this.target = target;
        this.transform = transform;
        this.bullet = bullet;
        this.mono = mono;
        this.weapon = weapon;
        this.stateDebugText = stateDebugText;
    }

    public void OnEnter()
    {
        Debug.Log("Attack");
        stateDebugText.text = "Attack";
        animator.CrossFade("Attack", 0f);
        animator.SetFloat("Speed", SPEED);
        animator.SetFloat("MotionSpeed", MOTION_SPEED);
        weapon.SetActive(true);
    }

    public void OnExit()
    {
        animator.SetTrigger("Move");
        weapon.SetActive(false);
        mono.StopCoroutine(ResetAttack());
    }

    public void Tick()
    {
        transform.LookAt(target);
    }

    public void AttackTarget()
    {
        if (!alreadyAtacked)
        {
            Rigidbody rb = Object.Instantiate(bullet, weapon.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 3f, ForceMode.Impulse);

            alreadyAtacked = true;

            mono.StartCoroutine(ResetAttack());
        }
    }

    IEnumerator ResetAttack()
    {
        animator.CrossFade("Attack", 0f);
        yield return new WaitForSeconds(TIMEBETWEENATTACK);
        alreadyAtacked = false;
    }
}
