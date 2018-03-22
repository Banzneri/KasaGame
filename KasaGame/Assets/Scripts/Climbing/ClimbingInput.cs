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
        return Input.GetKeyDown(KeyCode.LeftControl);
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

}
