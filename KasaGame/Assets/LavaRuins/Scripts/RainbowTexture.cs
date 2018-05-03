using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowTexture : MonoBehaviour
{

    #region Public Variables

    // Sprite Renderer
    [SerializeField]
    private SpriteRenderer _Renderer;

    // Speed of color change
    [SerializeField]
    private float Speed;

    // MIN and MAX values for RGB
    [SerializeField]
    private float SatMin, SatMax;

    #endregion

    #region Private Variables

    // Color values
    private float r, g, b;

    // Index of RGB that is changed
    private int RGB_Index;

    // Increasing or decreasing RGB value
    private bool ValueUp;

    #endregion

    #region Start & Update

    // Use this for initialization
    void Start()
    {

        if (_Renderer == null)
        {
            _Renderer = GetComponent<SpriteRenderer>();
        }

        r = 1;
        g = 0;
        b = 0;
        RGB_Index = 1;
        SetValueUp(false);
        SetColor();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
        SetColor();
    }

    #endregion

    #region Methods

    // main method
    private void ChangeColor()
    {

        int dir = 1;
        if (!ValueUp)
        {
            dir = -1;
        }

        SetRGB(GetRGB() + Speed * Time.deltaTime * dir);
        SetValueUp(true);
    }

    // Sets color to SpriteRenderer
    private void SetColor()
    {
        _Renderer.color = new Color(r, g, b, 1);
    }

    // Sets ValueUp to be correct
    private void SetValueUp(bool ChangeIndex)
    {
        bool ValueUpChanged = false;

        if (GetRGB() == SatMin)
        {
            ValueUp = true;
            ValueUpChanged = true;
        }
        else if (GetRGB() == SatMax)
        {
            ValueUp = false;
            ValueUpChanged = true;
        }

        if(ValueUpChanged && ChangeIndex)
        {
            RGB_Index--;
            if(RGB_Index < 0)
            {
                RGB_Index = 2;
            }
        }
    }

    // Returns correct RGB
    private float GetRGB()
    {
        if (RGB_Index == 0)
        {
            return r;
        }
        else if (RGB_Index == 1)
        {
            return g;
        }
        else if (RGB_Index == 2)
        {
            return b;
        }
        else
        {
            return 1;
        }
    }

    // Sets correct RGB
    private void SetRGB(float value)
    {
        if (RGB_Index == 0)
        {
            r = Mathf.Clamp(value, SatMin, SatMax);
        }
        else if (RGB_Index == 1)
        {
            g = Mathf.Clamp(value, SatMin, SatMax);
        }
        else if (RGB_Index == 2)
        {
            b = Mathf.Clamp(value, SatMin, SatMax);
        }
        else
        {
            //
        }
    }


    #endregion

}