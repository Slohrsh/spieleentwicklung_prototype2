using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNavigation : MonoBehaviour
{
    public float MinDist;
    public float Life;
    public LifeIndicator lifeIndicator;
    public float damage;

    private Animator animator;
    private NavMeshAgent agent;
    private Transform reference;
    private float initialLife;
    private bool isDead;

    // Use this for initialization
    void Start()
    {
        initialLife = Life;
        animator = this.GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        if(!isDead)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
            HandleMousInput();
            FollowAndAttack();
            CheckIfDead();
        }
    }

    private void CheckIfDead()
    {
        if (Life <= 0)
        {
            animator.SetBool("IsDead", true);
            Destroy(gameObject, 1f);
        }
    }

    private void HandleMousInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("UI")))
            {
                if (hit.transform.gameObject.CompareTag("Enemy"))
                {
                    reference = hit.transform;
                }
            }
            else if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Ground")))
            {
                agent.SetDestination(hit.point);
                reference = null;
            }
        }
    }

    public void decreaseLife(float damage)
    {
        if (Life > 0)
        {
            Life -= damage;
            lifeIndicator.decreaseLife(damage, initialLife);
        }
        else
        {
            lifeIndicator.Dead();
        }
    }

    private void FollowAndAttack()
    {
        if(reference != null)
        {
            if (Vector3.Distance(transform.position, reference.position) >= MinDist)
            {
                agent.destination = reference.position;
                transform.LookAt(reference);
            }
            else
            {
                agent.destination = transform.localPosition;
                EnemyController enemy = reference.GetComponent<EnemyController>();
                enemy.Damage(damage);
                animator.SetTrigger("Attack");
            }
        }
    }

    public void Damage(float damage)
    {
        decreaseLife(damage);
        animator.SetTrigger("Damage");
    }
}
