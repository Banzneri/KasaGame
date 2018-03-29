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

    // Whether there's still transition going on
    private bool _Transition;

    // target rotation for transition
    private Quaternion _TargetRotation;

    // target position for transition
    private Vector3 _TargetPosition;

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

        // initialize transition
        _Transition = true;
        bool Gradual = Host.IsGradual(_Edge);
        _TargetRotation = Host.RotationOnEdge(Gradual, _Edge);
        _TargetPosition = Host.PositionOnEdge(Gradual, _SidePosition, _Edge);
    }

    public override void Exit()
    {
        Host.GrabDelay = Time.deltaTime * 10;
        Host.PreviousEdge = _Edge;
        Host.Player.transform.rotation = Quaternion.LookRotation(_Edge.TransformToVectorInWorld(_Edge.ForwardDirection, true));
		Host.AnimatorComp.SetBool("IsClimbing", false);
    }

    public override void Run()
    {

        // If transitioning, do that and exit
        if (_Transition)
        {
            Transition();
            return;
        }
		
		Host.AnimatorComp.SetBool("IsMovingWhileClimbing", false);

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

        // if player hits Obstacles, let go of Edge
        if (!Host.SpaceOnPlayer(new Vector3(0.8f, 0.8f, 0.8f)))
        {
            Host.ChangeMode(new ModeOnAir(Host, false));
            return;
        }
    }

    #endregion

    #region Moving

    // Moves player to the edge
    private void Transition()
    {

        bool RotationReady = Quaternion.Angle(Host.Player.transform.rotation, _TargetRotation) < 25;
        bool MovingReady = Host.Player.transform.position == _TargetPosition;

        // check if transition is no more needed
        if (RotationReady && MovingReady)
        {
            _Transition = false;
            Host.AnimatorComp.SetBool("IsClimbing", true);
            return;
        }

        // rotation and moving step
        float step = 5 * Time.deltaTime;

        // rotate
        Host.Player.transform.rotation = Quaternion.Lerp(Host.Player.transform.rotation, _TargetRotation, step);

        // move
        Host.Player.transform.position = Vector3.MoveTowards(Host.Player.transform.position, _TargetPosition, step);
    }

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
            MoveHorizontal(-1);
        }
        else if (Host.Inputs.MoveRightHold())
        {
            MoveHorizontal(1);
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
			Host.AnimatorComp.SetBool("IsMovingWhileClimbing", true);
        }
        else
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
            Host.GrabOnEdge(_Edge, new Vector3(0.5f * Dir, 2, PosZ), new Vector3(1.5f, 1.5f, 1.5f));
            _SidePosition = Mathf.Clamp(_SidePosition, -0.5f, 0.5f);
        }

    }

    // DEPRECATED
    /* Move player on edge, vertical
    private void MoveVertical(int Dir)
    {
        bool EdgeFound;
        EdgeFound = Host.GrabOnEdge(_Edge, new Vector3(0, 2 + 0.75f * Dir, 0.5f), new Vector3(1.25f, 1.25f, 1.25f));

        // If no edge was found, check if player can stand up on the edge
        if (Dir == 1)
        {
            if (!EdgeFound && Host.SpaceOnTopOfEdge(_Edge))
            {
                Host.Player.transform.position = Host.WorldPointOnEdge(_SidePosition, _Edge) + _Edge.TransformToVectorInWorld(_Edge.ForwardDirection, true) * 0.3f;
                Host.ChangeMode(new ModeOnGround(Host));
            }
        }
    }*/

    #endregion
}