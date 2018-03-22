using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateOnAir : StateOfClimbing
{
    public StateOnAir(ClimbingPlugin host) : base(host)
    {

    }

    public override void EnterState()
    {
        Host.EnableDefaultControllingSystem(true);
    }

    public override void ExitState()
    {
        
    }

    public override void RunState()
    {
        // If player has returned to ground, change to ON_GROUND
        if (Host.VController.isGrounded)
        {
            Host.ChangeState(new StateOnGround(Host));
            return;
        }

        // If GrabDelay is positive, don't grab
        if(Host.GrabDelay > 0)
        {
            return;
        }

        // Find Edges inside GrabCollider
        Collider[] GrabEdges = Host.OverlapSphere(Host.GrabCollider, Host.EdgeLayer);

        // If found Edge, pick first
        if (GrabEdges.Length > 0)
        {
            Host.ChangeState(new StateOnLedge(Host, GrabEdges[0].GetComponent<EdgeBehaviour>()));
            return;
        }



    }
}
