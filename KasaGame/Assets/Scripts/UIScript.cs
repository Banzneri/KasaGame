using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

    public Door door;
    [SerializeField] private Image key;

    public void Update()
    {
        if(door.doorKey == false)
        {
            key.enabled = false;
        }
        else
        {
            key.enabled = true;
        }
    }
}
