using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateOnLedge : StateOfClimbing
{

    #region Variables

    // Ledge
    private EdgeBehaviour _Ledge;
    public EdgeBehaviour Ledge
    {
        get { return _Ledge; }
    }

    // Player's position from Ledge's center
    private float _DifferenceX;
    public float DifferenceX
    {
        get { return _DifferenceX; }
    }

    #endregion

    #region Start and End

    public StateOnLedge(ClimbingPlugin host, EdgeBehaviour ledge) : base(host)
    {
        _Ledge = ledge;
    }

    public override void EnterState()
    {
        Host.EnableDefaultControllingSystem(false);
        _DifferenceX = Ledge.TransformToEdge(Host.Player.transform.position);
        SetPlayerPositionAndRotation();
    }

    public override void ExitState()
    {
        Host.GrabDelay = 0.2f;
    }

    #endregion

    #region Interacting with Ledge

    // Get ledge direction in Global Space. If Straightened, Y value will be zero (so that direction is only on 2D XZ space)
    private Vector3 DirectionGlobal(Vector3 Direction, bool Straighten)
    {
        if (Straighten)
        {
            Direction = Ledge.transform.TransformDirection(Direction);
            Direction.Set(Direction.x, 0, Direction.z);
            return Direction;
        }
        else
        {
            return Ledge.transform.TransformDirection(Direction);
        }
    }

    // Sets player's position and rotation on ledge
    private void SetPlayerPositionAndRotation()
    {
        Host.Player.transform.rotation = Quaternion.LookRotation(DirectionGlobal(Ledge.FacingDirection, true));
        Host.Player.transform.position = Ledge.PointOnEdge(_DifferenceX) + DirectionGlobal(Ledge.FacingDirection, true) * -0.5f + DirectionGlobal(Ledge.UpDirection, false) * -1.5f;
    }

    // Sets DifferenceX and restricts it to be on the corners of Ledge
    private void MovePlayerOnLedge(float Value)
    {
        // Where player would be if moved
        Vector3 PossiblePosition = Ledge.PointOnEdge(Value);

        // Difference between new and current position
        Vector3 Difference = PossiblePosition - Host.Player.transform.position;

        // If in new position player would hit wall, don't move. Check if there's another grab to hang from
        if (Host.CheckSphere(Host.PhysicsCollider, Vector3.zero, Host.ObstacleLayer))
        {
            GoAroundCorner();
            return;
        }

        // If no walls, check if current ledge is still continuing
        if (Ledge.IsOnLedge(PossiblePosition))
        {
            _DifferenceX = Value;
        }
        else
        {
            // In this case player attempts to go over ledge. Check if there's another ledge to hang from
            GoAroundCorner();
        }
    }

    // Attempt to move to another ledge that is behind corner
    private void GoAroundCorner()
    {

        // Find Edges inside GrabCollider
        Collider[] GrabEdges = Host.OverlapSphere(Host.GrabCollider, Host.EdgeLayer);

        // If found Edge, pick first edge that is not current edge
        for (int i = 0; i < GrabEdges.Length; i++)
        {
            if (GrabEdges[i].GetComponent<EdgeBehaviour>().Equals(Ledge))
            {
                continue;
            }

            EdgeBehaviour edge = GrabEdges[i].GetComponent<EdgeBehaviour>();

            // Check whether player would be inside Obstacle if changing to this ledge
            float DiffX = edge.TransformToEdge(Host.Player.transform.position);
            Host.Player.transform.rotation = Quaternion.LookRotation(DirectionGlobal(edge.FacingDirection, true));
            Host.Player.transform.position = edge.PointOnEdge(DiffX) + DirectionGlobal(edge.FacingDirection, true) * -0.5f + DirectionGlobal(edge.UpDirection, false) * -1.5f;
            if(Host.CheckSphere(Host.PhysicsCollider, Vector3.zero, Host.ObstacleLayer))
            {
                SetPlayerPositionAndRotation();
                continue;
            }

            // if there was no problems, change ledge
            Host.ChangeState(new StateOnLedge(Host, edge));
            return;
        }
    }

    // Attempts to stand up on the ledge
    private void StandUp()
    {
        if (!Host.CheckSphere(Host.RoomUpCollider, Vector3.zero, Host.ObstacleLayer))
        {
            Host.Player.transform.position = Ledge.PointOnEdge(_DifferenceX) + DirectionGlobal(Ledge.FacingDirection, true) * 0.25f + DirectionGlobal(Ledge.UpDirection, false) * 0.2f;
            Host.ChangeState(new StateOnGround(Host));
        }
    }

    #endregion

    public override void RunState()
    {
        // If Grab button is pressed, player falls down
        if (Input.GetKey(KeyCode.Q) || Input.GetKeyDown(KeyCode.S))
        {
            Host.ChangeState(new StateOnAir(Host));
            return;
        }

        // If Ledge is not climbable anymore, let go
        if (!Ledge.Climbable(Host.MaxGradientEdge, Host.MaxGradientFacing))
        {
            Host.ChangeState(new StateOnAir(Host));
            return;
        }

        // Set player position and rotation
        SetPlayerPositionAndRotation();

        // INPUTS
        if (Input.GetKey(KeyCode.A))
        {
            MovePlayerOnLedge(DifferenceX - Time.deltaTime * 3);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            MovePlayerOnLedge(_DifferenceX + Time.deltaTime * 3);
        }
        
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            StandUp();
        }
    }

}
