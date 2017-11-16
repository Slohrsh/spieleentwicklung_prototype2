using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeIndicator : MonoBehaviour {

    public float life;
    private float initialLife;
    private float actualDamageInScale;

    // Use this for initialization
    void Start () {
        initialLife = life;
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }
    float deltaTime;
    // Update is called once per frame
    void Update () {
        deltaTime += Time.deltaTime;
        if(deltaTime > 1)
        {
            deltaTime = 0;
            decreaseLife(20);
        }
    }
    public void decreaseLife(float damage)
    {
        if(life > 0)
        {
            life -= damage;
            float scaledDamage = ((100 / initialLife) * damage)/100;
            actualDamageInScale += scaledDamage;
            transform.localScale += new Vector3(-1*scaledDamage, 0, 0);
            changeColor();
        }
        else
        {
            //lost
        }
    }

    private void changeColor()
    {
        Debug.Log(actualDamageInScale);
        if (actualDamageInScale < 0.40)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        if (actualDamageInScale > 0.70)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        if (actualDamageInScale < 0.70 && actualDamageInScale > 0.40)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }
}
