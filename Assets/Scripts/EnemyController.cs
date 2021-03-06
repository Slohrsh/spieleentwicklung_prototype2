﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public Texture2D cursorTexture;
    public Rigidbody player;
    public float MinDist;
    public float TriggerDist;
    public float Life;
    public LifeIndicator lifeIndicator;
    public float damage;
    public bool dropKey;
    public GameObject dropedKeyPrefab;

    private bool isDead = false;
    private Animator animator;
    private NavMeshAgent agent;
    private float initialLife;

    void Start()
    {
        initialLife = Life;
        animator = this.GetComponentInChildren<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!isDead)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
            FollowPlayerIfReachable();
            CheckIfDead();
        }
    }

    private void CheckIfDead()
    {
        if (Life <= 0)
        {
            isDead = true;
            animator.SetBool("IsDead", isDead);
            CheckGround();
            Destroy(gameObject, 2f);
            DropKey();
            OnMouseExit();
        }
    }

    private void DropKey()
    {
        if (dropKey)
        {
            GameObject droppedItem;
            droppedItem = Instantiate(dropedKeyPrefab, transform.position, transform.rotation);
            droppedItem.SetActive(true);
        }
    }

    private float deltaTime;
    private void FollowPlayerIfReachable()
    {
        if(player != null)
        {
            if (Vector3.Distance(transform.position, player.position) <= TriggerDist)
            {
                agent.destination = player.position;
                transform.LookAt(player.transform);
            }
            if (Vector3.Distance(transform.position, player.position) <= MinDist)
            {
                agent.destination = transform.localPosition;
                PlayerNavigation nav = player.GetComponent<PlayerNavigation>();
                deltaTime += Time.deltaTime;
                if (deltaTime >= 1)
                {
                    nav.Damage(damage);
                    animator.SetTrigger("Attack");
                    deltaTime = 0;
                }
            }
        }
    }

    private void CheckGround()
    {
        int rockMask = 1 << NavMesh.GetAreaFromName("Rock");
        NavMeshHit hit;
        agent.SamplePathPosition(-1, 0.0f, out hit);
        if (hit.mask == rockMask)
        {
            agent.speed = 3;
        }
        else
        {
            agent.speed = 10;
        }
    }

    public void Damage(float damage)
    {
        decreaseLife(damage);
        animator.SetTrigger("Damage");
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
    void OnMouseOver()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }


}
