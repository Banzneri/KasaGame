using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingInput {

	public ClimbingInput()
    {

    }

    // Returns true if Release button is pressed down in this frame
    public bool Release()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }

    // Returns true if Release button is pressed
    public bool ReleaseHold()
    {
        return Input.GetKey(KeyCode.Q);
    }

    // Returns true if Move Left is pressed
    public bool MoveLeftHold()
    {
        return Input.GetKey(KeyCode.A);
    }

    // Returns true if Move Right is pressed
    public bool MoveRightHold()
    {
        return Input.GetKey(KeyCode.D);
    }

    // Returns true if Jump button is pressed down in this frame
    public bool Jump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    // Returns true if Move Up is pressed down in this frame
    public bool MoveUp()
    {
        return Input.GetKeyDown(KeyCode.W);
    }

    // Returns true if Move Down is pressed down in this frame
    public bool MoveDown()
    {
        return Input.GetKeyDown(KeyCode.S);
    }

}
