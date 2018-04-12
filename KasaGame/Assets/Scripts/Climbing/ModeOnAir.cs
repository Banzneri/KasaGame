using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeOnAir : ClimbingMode
{

    // jump
    private bool Jump;

    public ModeOnAir(ClimbingBehaviour host, bool jump) : base(host)
    {
        Jump = jump;
    }

    public override void Enter()
    {
        Host.EnableDefaultControllingSystem(true);
        
        if (Jump)
        {
            Host.Player.GetComponent<JumpManager>().EdgeJump();
            Host.VController.isGrounded = true;
            Host.VController.Jump();
            Host.VController.isGrounded = false;
            Jump = false;
        }
    }

    public override void Exit()
    {
        Host.GrabDelay = 0;
    }

    public override void Run()
    {

        // If player has returned to ground, change to ON_GROUND
        if (Host.VController.isGrounded)
        {
            Host.ChangeMode(new ModeOnGround(Host));
            return;
        }

        // Try to grab from an edge
        Host.GrabOnAir();
    }
}
