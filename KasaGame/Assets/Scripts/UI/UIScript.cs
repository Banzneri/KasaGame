using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

    public Door goldDoor;
    [SerializeField] private Image goldKey;

    public void Update()
    {
        if(goldDoor.doorKey == false)
        {
            goldKey.enabled = false;
        }
        else
        {
            goldKey.enabled = true;
        }
    }
}
