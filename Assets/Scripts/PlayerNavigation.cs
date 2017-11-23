using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNavigation : MonoBehaviour
{
    public float MinDist;
    public float Life;
    public LifeIndicator lifeIndicator;

    private Animator animator;
    private NavMeshAgent agent;
    private Transform reference;
    private float initialLife;

    // Use this for initialization
    void Start()
    {
        initialLife = Life;
        animator = this.GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    private float deltaTime;
    void Update()
    {

        deltaTime += Time.deltaTime;
        if (deltaTime > 1)
        {
            deltaTime = 0;
            decreaseLife(20);
        }

        HandleMousInput();
        FollowAndAttack();
        

        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (Life <= 0)
        {
            animator.SetBool("IsDead", true);
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
                    Debug.Log("Follow enemy!");
                    reference = hit.transform;
                }
            }
            else if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Ground")))
            {
                Debug.Log("Hit: " + hit.collider.name);
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
            agent.destination = reference.position;
            if(Vector3.Distance(transform.position, reference.position) <= MinDist)
            {
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
