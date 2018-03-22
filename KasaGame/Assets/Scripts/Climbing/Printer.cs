using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : MonoBehaviour
{

    public bool Position;
    public bool Rotation;
    public bool Euler;
    public float PrintSpeed;
    public bool AutomaticPrint;
    public bool ManualPrint;

    private float _Timer;

    // Use this for initialization
    void Start()
    {
        _Timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (ManualPrint)
        {
            Print();
            ManualPrint = false;
        } else if(AutomaticPrint)
        {
            _Timer += Time.deltaTime;

            if (_Timer >= PrintSpeed)
            {
                Print();
                _Timer = 0;
            }
        }
    }

    private void Print()
    {
        if (Rotation)
        {
            Debug.Log("Rotation: " + transform.rotation + ", Local: " + transform.localRotation);
        }

        if (Euler)
        {
            Debug.Log("Euler: " + transform.eulerAngles + ", Local: " + transform.localEulerAngles);
        }

        if (Position)
        {
            Debug.Log("Position: " + transform.position + ", Local: " + transform.localPosition);
        }
    }

}
