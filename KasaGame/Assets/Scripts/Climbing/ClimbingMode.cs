using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClimbingMode {

    // ClimbingBehaviour
    private ClimbingBehaviour _Host;
    public ClimbingBehaviour Host
    {
        get { return _Host; }
    }


    public ClimbingMode(ClimbingBehaviour host)
    {
        _Host = host;
    }

    // Called when the status is initialized
    public abstract void Enter();

    // Called when the status is active
    public abstract void Run();

    // Called when the status is finalized
    public abstract void Exit();

}
