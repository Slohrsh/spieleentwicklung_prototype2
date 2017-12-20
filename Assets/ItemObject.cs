using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour {

    public int Amount;
    public Image Texture;
    public string name { get { return GetComponent<GameObject>().tag; } }

    public bool HasItem()
    {
        return Amount > 0;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
