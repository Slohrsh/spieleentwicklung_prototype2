using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNavigation : MonoBehaviour
{
    public float MinDist;
    public float Life;
    public LifeIndicator lifeIndicator;
    public float damage;
    public float destroyabledistance = 20;
    public GameManager gameManager;

    private Animator animator;
    private NavMeshAgent agent;
    private Transform enemyReference;
    private Transform destroyableReference;
    private float initialLife;
    private bool isDead;
    private Inventory inventory;

    void Start()
    {
        initialLife = Life;
        animator = this.GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        if(!isDead)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
            HandleMousInput();
            FollowAndAttack();
            CheckIfDead();
            CheckGround();
        }
    }

    private void CheckGround()
    {
        int rockMask = 1 << NavMesh.GetAreaFromName("Rock");
        NavMeshHit hit;
        agent.SamplePathPosition(-1, 0.0f, out hit);
        if (hit.mask == rockMask)//changed line
            if (inventory.HasAxe == false)
                agent.speed = 3;
            else
                agent.speed = 10;
        else
            agent.speed = 10;

    }

    private void CheckIfDead()
    {
        if (Life <= 0)
        {
            animator.SetBool("IsDead", true);
            Destroy(gameObject, 3f);
            Invoke("GameOver", 3f);
        }
    }

    public void Damage(float damage)
    {
        decreaseLife(damage);
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

    private void GameOver()
    {
        gameManager.GameOver();
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
                    enemyReference = hit.transform;
                }
                else if (hit.transform.gameObject.CompareTag("Destroyable"))
                {
                    destroyableReference = hit.transform;
                }
                else if (hit.transform.gameObject.CompareTag("Destroyable_KeyRequired"))
                {
                    if(inventory.HasKey)
                    {
                        Destroy(hit.transform.gameObject);
                        inventory.DecreaseItem(Inventory.KEY);
                    }
                }
            }
            else if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Ground")))
            {
                agent.SetDestination(hit.point);
                enemyReference = null;
                destroyableReference = null;
            }
        }
    }

    private float deltaTime;
    private void FollowAndAttack()
    {
        if(enemyReference != null)
        {
            if (Vector3.Distance(transform.position, enemyReference.position) >= MinDist)
            {
                agent.destination = enemyReference.position;
                transform.LookAt(enemyReference);
            }
            else
            {
                agent.destination = transform.localPosition;
                EnemyController enemy = enemyReference.GetComponent<EnemyController>();
                deltaTime += Time.deltaTime;
                if(deltaTime >= 1)
                {
                    animator.SetTrigger("Attack");
                    enemy.Damage(damage);
                    deltaTime = 0;
                }
            }
        }
        else if(destroyableReference != null)
        {
            if (Vector3.Distance(transform.position, destroyableReference.position) >= destroyabledistance)
            {
                agent.destination = destroyableReference.position;
                transform.LookAt(destroyableReference);
            }
            else
            {
                agent.destination = transform.localPosition;
                Destroyable destroyable = destroyableReference.GetComponent<Destroyable>();
                deltaTime += Time.deltaTime;
                if (deltaTime >= 1)
                {
                    animator.SetTrigger("Attack");
                    destroyable.Damage(damage);
                    deltaTime = 0;
                }
            }
        }
    }
}
