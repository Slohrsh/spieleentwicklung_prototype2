using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour {

    public Inventory inventory;


    private Button[] buttons;
	// Use this for initialization
	void Start () {
        buttons = GetComponentsInChildren<Button>();
        foreach(Button button in buttons)
        {
            button.onClick.AddListener(() => onButtonClick(button));
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        for(int i = 1; i <= 4 && i <= inventory.Items.Count; i++)
        {
            Debug.Log(buttons[i].tag);
            buttons[i].image = inventory.Items[i].Texture;
        }
	}
    
    public void onButtonClick(Button button)
    {
        Debug.Log(button.tag);
        switch (button.tag)
        {
            case "Left":
                break;
            case "Right":
                break;
            case "Slot1":
                break;
            case "Slot2":
                break;
            case "Slot3":
                break;
            case "Slot4":
                break;
        }
    }

}
    