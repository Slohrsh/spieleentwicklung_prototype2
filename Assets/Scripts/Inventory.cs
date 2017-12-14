using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public List<Item> items;

    public const String LIFEPOT = "LifePot";
    public const String KEY = "Key";
    public const String ICE_AXE = "IceAxe";
    public const String BARREL = "Barrel";


    public void Start()
    {
        
    }

    public void DecreaseItem(String tag)
    {
        manipulateItem(tag, -1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(manipulateItem(other.tag, 1))
        {
            Destroy(other.gameObject);
        }
    }

    private bool manipulateItem(String tag, int value)
    {
        bool isItem = false;
        foreach(Item item in items)
        {
            if(item.name.Equals(tag))
            {
                item.Amount += value;
                isItem = true;
            }
        }
        return isItem;
    }

}
