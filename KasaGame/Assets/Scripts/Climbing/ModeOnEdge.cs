using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeOnEdge : ClimbingMode
{

    #region Variables

    // The Edge player is hanging from
    private Edge _Edge;

    // Difference from the center of the Edge in SideDirection
    private float _SidePosition;

    // If player found new edge
    private bool ChangeEdge;

    #endregion

    #region Main methods

    public ModeOnEdge(ClimbingBehaviour host, Edge edge) : base(host)
    {
        _Edge = edge;
    }

    public override void Enter()
    {
        Host.EnableDefaultControllingSystem(false);
        _SidePosition = Host.SidePositionOnEdge(Host.Player.transform, _Edge, true);
        Host.SetPlayerToEdge(_SidePosition, _Edge);
        Host.Player.gameObject.GetComponent<Animator>().SetBool("IsClimbing", true);
    }

    public override void Exit()
    {
        Host.GrabDelay = Time.deltaTime * 10;
        Host.PreviousEdge = _Edge;
        Host.Player.transform.rotation = Quaternion.LookRotation(_Edge.TransformToVectorInWorld(_Edge.ForwardDirection, true));
        Host.Player.gameObject.GetComponent<Animator>().SetBool("IsClimbing", false);
    }

    public override void Run()
    {
        ChangeEdge = false;

        // If _Edge is not climbable anymore, player falls down
        if (!_Edge.Climbable(Host.MaxGradientSide, Host.MaxGradientForward))
        {
            Host.ChangeMode(new ModeOnAir(Host, false));
            return;
        }

        // Set player's position and rotation
        Host.SetPlayerToEdge(_SidePosition, _Edge);

        // Inputs
        HandleInputs();

        // if new edge found, don't continue
        if (ChangeEdge)
        {
            return;
        }

        // if player hits Obstacles, let go of Edge
        if (!Host.SpaceOnPlayer(new Vector3(1, 1, 1)))
        {
            Host.ChangeMode(new ModeOnAir(Host, false));
            return;
        }
    }

    #endregion

    #region Moving

    // Checks for inputs
    private void HandleInputs()
    {

        // Let go of edge
        if (Host.Inputs.Release())
        {
            Host.ChangeMode(new ModeOnAir(Host, false));
            return;
        }

        // jump from edge
        if (Host.Inputs.Jump())
        {
            Host.ChangeMode(new ModeOnAir(Host, true));
            return;
        }

        // Move alongside SideDirection
        if (Host.Inputs.MoveLeftHold())
        {
            Host.Player.gameObject.GetComponent<Animator>().SetBool("IsMovingWhileClimbing", true);
            MoveHorizontal(-1);
        }
        else if (Host.Inputs.MoveRightHold())
        {
            Host.Player.gameObject.GetComponent<Animator>().SetBool("IsMovingWhileClimbing", true);
            MoveHorizontal(1);
        }
        else
        {
            Host.Player.gameObject.GetComponent<Animator>().SetBool("IsMovingWhileClimbing", false);
        }

        // Move down, move up / stand
        if (Host.Inputs.MoveUp())
        {
            MoveVertical(1);
        } else if (Host.Inputs.MoveDown())
        {
            MoveVertical(-1);
        } else
        {
        }

    }

    // Move player on edge, horizontal
    private void MoveHorizontal(int Dir)
    {

        // calculate new SidePosition
        float NewSidePos = _SidePosition + (Time.deltaTime * Host.MoveSpeed / _Edge.SideDirectionScale()) * Dir;

        // is there need to check for new edge
        bool GrabEdge = false;

        // GrabCollider's Z position
        float PosZ = 0.5f;

        // If there's room, move
        if (Host.SpaceOnEdge(_Edge, NewSidePos))
        {
            _SidePosition = NewSidePos;
        } else
        {
            // if not, check for new edge
            GrabEdge = true;
            PosZ = 0;
        }

        // If SidePosition is over edge, check for new edge
        if (_SidePosition > 0.5f || _SidePosition < -0.5f)
        {
            GrabEdge = true;
        }

        // find new edge
        if (GrabEdge)
        {
            ChangeEdge = Host.GrabOnEdge(_Edge, new Vector3(0.5f * Dir, 2, PosZ), new Vector3(1.5f, 1.5f, 1.5f));
            _SidePosition = Mathf.Clamp(_SidePosition, -0.5f, 0.5f);
        }

    }

    // Move player on edge, vertical
    private void MoveVertical(int Dir)
    {
        bool EdgeFound;
        EdgeFound = Host.GrabOnEdge(_Edge, new Vector3(0, 2 + 0.75f * Dir, 0.5f), new Vector3(1.25f, 1.25f, 1.25f));

        // If no edge was found, check if player can stand up on the edge
        if (Dir == 1)
        {
            if (!EdgeFound && Host.SpaceOnTopOfEdge(_Edge))
            {
                ChangeEdge = true;
                Host.Player.transform.position = Host.WorldPointOnEdge(_SidePosition, _Edge) + _Edge.TransformToVectorInWorld(_Edge.ForwardDirection, true) * 0.3f;
                Host.ChangeMode(new ModeOnGround(Host));
            }
        }
    }

    #endregion
}