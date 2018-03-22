using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateOnGround : StateOfClimbing
{
    public StateOnGround(ClimbingPlugin host) : base(host)
    {

    }

    public override void EnterState()
    {
        Host.EnableDefaultControllingSystem(true);

    }

    public override void ExitState()
    {
        LockPlayer(false);
    }

    public override void RunState()
    {
        // If player in on air, change to ON_AIR
        if (!Host.VController.isGrounded)
        {
            Host.ChangeState(new StateOnAir(Host));
            return;
        }

        // Stop player to the edge if needed
        LockPlayer(StopOnEdge());

    }

    // Returns true if player should not move
    private bool StopOnEdge()
    {

        // If player is pressing walk button, continue
        if(!Input.GetKey(KeyCode.LeftShift))
        {
            return false;
        }

        // If player is standing on the edge, continue
        if (!Host.CheckSphere(Host.FeetCollider, Vector3.zero, Host.EdgeLayer))
        {
            return false;
        }

        // If there's empty in front of player, stop player
        if(Host.CheckSphere(Host.EmptyCollider, Vector3.zero, Host.ObstacleLayer))
        {
            return false;
        }


        return true;
    }

    // Locks or unlocks movement of player
    private void LockPlayer(bool LockPlayer)
    {
        //Debug.Log("Lock: " + LockPlayer);
        // Make the player stand still
        Host.EnableDefaultControllingSystem(!LockPlayer);
    }

}
