using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SelectOnInput : MonoBehaviour {

    public EventSystem eventSystem; 
    public GameObject selectedObject;

    private bool buttonSelected;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw ("Vertical") != 0 && buttonSelected == false) //Detect keyboard moviment up or down
        {
            eventSystem.SetSelectedGameObject(selectedObject); //Select one button
            buttonSelected = true;
        }
	}

    private void onDisable()
    {
        buttonSelected = false;
    }
}
