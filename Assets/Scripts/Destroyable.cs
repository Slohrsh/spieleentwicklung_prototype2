using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour {

    public Texture2D cursorTexture;
    public float Life;

    void Start()
    {

    }

    void Update()
    {
        if(Life <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Damage(float damage)
    {
        Life -= damage;
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
