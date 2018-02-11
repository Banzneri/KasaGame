using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, ITriggerObject<IActionObject> {
    [SerializeField]
    private GameObject inspectorObject; //This is needed for actionObject to show up in the inspector
    private IActionObject actionObject; //The object this button triggers. (door, platform etc.)
    private bool inTrigger;

    void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }

    private void Update()
    {
        if(inTrigger)
        {
            if(Input.GetButtonDown("action"))
            {
                Trigger(actionObject);
            }
        }
    }

    //This is needed for actionObject to show up in the inspector
    void OnValidate()
    {
        actionObject = inspectorObject.GetComponent<IActionObject>();
        if (actionObject == null) inspectorObject = null;
    }

    public void Trigger(IActionObject actionObject)
    {
        actionObject.Action();
    }

    private void OnGUI()
    {
        if (inTrigger)
        {
                GUI.Box(new Rect(450, 400, 200, 25), "Press E to interact");
        }
    }
}
