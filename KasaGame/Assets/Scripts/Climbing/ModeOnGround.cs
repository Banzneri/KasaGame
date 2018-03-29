using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeOnGround : ClimbingMode
{
    public ModeOnGround(ClimbingBehaviour host) : base(host)
    {

    }

    public override void Enter()
    {
        Host.EnableDefaultControllingSystem(true);
        Host.GrabDelay = 0;
        Host.VController.jumpAirControl = true;
    }

    public override void Exit()
    {

    }

    public override void Run()
    {

        // If player in on air, change to ON_AIR
        if (!Host.VController.isGrounded)
        {
            Host.ChangeMode(new ModeOnAir(Host, false));
            return;
        }

    }
   
}