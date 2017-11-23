using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {


    public Texture2D cursorTexture;
    Animator animator;
    NavMeshAgent agent;

    void Start()
    {
        animator = this.GetComponentInChildren<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        agent.SetDestination(Vector3.zero);
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
