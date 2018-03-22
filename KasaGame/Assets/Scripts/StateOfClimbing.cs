using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateOfClimbing {

    // ClimbingPlugin
    private ClimbingPlugin _Host;
    public ClimbingPlugin Host
    {
        get { return _Host; }
    }


    public StateOfClimbing(ClimbingPlugin host)
    {
        _Host = host;
    }

    // Called when the state is initialized
    public abstract void EnterState();

    // Called when the state is active
    public abstract void RunState();

    // Called when the state is finalized
    public abstract void ExitState();

}
