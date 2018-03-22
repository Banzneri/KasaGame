using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class ClimbingBehaviour : MonoBehaviour {

    #region Variables

    #region Player Character
    [Header("Player Character")]

    // Player Character gameobject
    [SerializeField]
    private GameObject _Player;
    public GameObject Player
    {
        get { return _Player; }
    }

    // Ghost of Player character, used to calculate possible positions
    [SerializeField]
    private GameObject _PlayerGhost;

    // V Third Person Controller attached to player
    [SerializeField]
    private vThirdPersonController _VController;
    public vThirdPersonController VController
    {
        get { return _VController; }
    }

    // V Third Person Input attached to player
    [SerializeField]
    private vThirdPersonInput _VInput;
    public vThirdPersonInput VInput
    {
        get { return _VInput; }
    }

    // Rigidbody attached to player
    [SerializeField]
    private Rigidbody _Rigidbody;
	
	// Animator Component
	[SerializeField]
	private Animator _AnimatorComp;
	public Animator AnimatorComp {
		get { return _AnimatorComp; }
	}

    #endregion

    #region Climbing Settings
    [Header("Climbing Settings")]

    // LayerMask for all Obstacles
    [SerializeField]
    private LayerMask _ObstacleLayer;
    public LayerMask ObstacleLayer
    {
        get { return _ObstacleLayer; }
    }

    // LayerMask for all Frames
    [SerializeField]
    private LayerMask _FrameLayer;
    public LayerMask FrameLayer
    {
        get { return _FrameLayer; }
    }

    // LayerMask for all Edges
    [SerializeField]
    private LayerMask _EdgeLayer;
    public LayerMask EdgeLayer
    {
        get { return _EdgeLayer; }
    }

    // Maximum gradient on side direction
    [SerializeField]
    private float _MaxGradientSide;
    public float MaxGradientSide
    {
        get { return _MaxGradientSide; }
    }

    // Maximum gradient on forward direction
    [SerializeField]
    private float _MaxGradientForward;
    public float MaxGradientForward
    {
        get { return _MaxGradientForward; }
    }

    // Difference from edge Y pos when hanging
    [SerializeField]
    private float _EdgeGrabDifferenceY;

    // Difference from edge Z pos when hanging
    [SerializeField]
    private float _EdgeGrabDifferenceZ;

    // Speed when hanging
    [SerializeField]
    private float _MoveSpeed;
    public float MoveSpeed
    {
        get { return _MoveSpeed; }
    }

    #endregion

    #region Collider Detectors
    [Header("Collider Detectors")]

    // Used to check if player can grab from an eedge
    [SerializeField]
    private SphereCollider _GrabCollider;

    // Used to check if there's enough space for the player
    [SerializeField]
    private SphereCollider _PhysicsCollider;

    #endregion

    #region Privates

    // Current mode of player
    private ClimbingMode _CurrentMode;

    // List of Frames found in Update
    private Collider[] _Frames;
    public Collider[] Frames
    {
        get { return _Frames; }
    }

    // Delay that prevents player from grabbing same Edge right after leaving it
    private float _GrabDelay;
    public float GrabDelay
    {
        get { return _GrabDelay; }
        set { _GrabDelay = value; }
    }

    // Previous Edge, used to prevent player from grabbing this when there's grab delay
    private Edge _PreviousEdge;
    public Edge PreviousEdge
    {
        get { return _PreviousEdge; }
        set { _PreviousEdge = value; }
    }

    // Class for inputs
    private ClimbingInput _Inputs;
    public ClimbingInput Inputs
    {
        get { return _Inputs; }
    }

    #endregion

    #endregion

    #region Initialization and Update

    // Use this for initialization
    void Start () {
        transform.localPosition = new Vector3(0, 0, 0);
        FindPlayer();
        _CurrentMode = new ModeOnAir(this, false);
        _Inputs = new ClimbingInput();
    }

    // Finds player character and scripts attached to it
    private void FindPlayer()
    {

        // find PlayerCharacter if missing
        if (_Player == null)
        {
            _Player = transform.parent.gameObject;
            if (_Player == null)
            {
                Debug.LogError("Player not found");
            }
        }

        // Find VThirdPersonController if missing
        if (_VController == null)
        {
            _VController = _Player.GetComponent<vThirdPersonController>();
            if (_VController == null)
            {
                Debug.LogError("V Third Person Controller not found");
            }
        }

        // Find VThirdPersonInput if missing
        if (_VInput == null)
        {
            _VInput = _Player.GetComponent<vThirdPersonInput>();
            if (_VInput == null)
            {
                Debug.LogError("V Third Person Input not found");
            }
        }

        // Find Rigidbody if missing
        if (_Rigidbody == null)
        {
            _Rigidbody = _Player.GetComponent<Rigidbody>();
            if (_Rigidbody == null)
            {
                Debug.LogError("Rigidbody not found");
            }
        }
		
		 // Find Animator if missing
        if (_AnimatorComp == null)
        {
            _AnimatorComp = _Player.GetComponent<Animator>();
            if (_AnimatorComp == null)
            {
                Debug.LogError("Animator not found");
            }
        }
    }

    // Update is called once per frame
    void LateUpdate () {

        // Find Frames nearby and update their Edges
        _Frames = Physics.OverlapSphere(transform.position, 2.5f, _FrameLayer);
        for (int i = 0; i < _Frames.Length; i++)
        {
            _Frames[i].GetComponent<Frame>().UpdateEdges(_MaxGradientSide, _MaxGradientForward);
        }

        // Run mode
        _CurrentMode.Run();

        // Disable Edges of found Frames
        for (int i = 0; i < _Frames.Length; i++)
        {
            _Frames[i].GetComponent<Frame>().DisableEdges();
        }
    }

    // Enable default controlling scripts and components
    public void EnableDefaultControllingSystem(bool enable)
    {
        _VController.enabled = enable;
        _VController.lockMovement = !enable;
        //_VInput.enabled = enable;
        _Rigidbody.isKinematic = !enable;

        if (!enable)
        {
            _VController.isJumping = false;
        }
    }

    #endregion

    #region Modes

    // Calls Exit of current Mode and Enter of new Mode
    public void ChangeMode(ClimbingMode mode)
    {
        _CurrentMode.Exit();
        _CurrentMode = mode;
        _CurrentMode.Enter();
    }

    #endregion

    #region Collider checks

    #region Physics

    // Performs Physics.CheckSphere with given parameters and returns result
    private bool CheckSphere(SphereCollider collider, Vector3 Offset, LayerMask layer)
    {
        return Physics.CheckSphere(collider.bounds.center + Offset, collider.bounds.size.y / 2, layer);
    }

    // Performs Physics.OverlapSphere with given parameters and returns result
    private Collider[] OverlapSphere(SphereCollider collider, LayerMask layer)
    {
        return Physics.OverlapSphere(collider.bounds.center, collider.bounds.size.y / 2, layer);
    }

    #endregion

    #region Checks with GrabCollider

    // Performs Check with GrabCollider, finds best edge to grab from if there is any
    private bool GrabEdge(Edge Current)
    {
        ResetPlayerGhost();

        // Find Edges in GrabCollider
        Collider[] Colliders = OverlapSphere(_GrabCollider, EdgeLayer);

        // Array of Edges
        List<Edge> Edges = new List<Edge>();

        // Find all suitable edges
        for(int i = 0; i < Colliders.Length; i++)
        {
            Edge edge = Colliders[i].GetComponent<Edge>();

            // Ignore current edge
            if (edge.Equals(Current))
            {
                continue;
            }

            // Add edge to the list if it's available
            if(SpaceOnEdge(edge, SidePositionOnEdge(_PlayerGhost.transform, edge, true)))
            {
                Edges.Add(edge);
            }
        }

        return FindBestEdge(Edges);
    }

    // Sets GrabCollider ready for normal check on air
    public void GrabOnAir()
    {
        _GrabCollider.transform.localPosition = new Vector3(0, 2, 0.5f);
        _GrabCollider.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        // If player has just let go from previous edge, don't immediately grab another
        if (GrabDelay > 0)
        {
            GrabDelay -= Time.deltaTime;
        } else
        {
            GrabEdge(null);
        }
    }

    // DEPRECATED
    /* Sets GrabCollider ready for normal check on ground
    public void GrabOnGround()
    {
        _GrabCollider.transform.localPosition = new Vector3(0, 0.25f, 0.25f);
        _GrabCollider.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        GrabEdge(null);
    }*/

    // Sets GrabCollider ready for normal check on air
    public bool GrabOnEdge(Edge Current, Vector3 GrabDirection, Vector3 GrabScale)
    {
        _GrabCollider.transform.localPosition = GrabDirection;
        _GrabCollider.transform.localScale = GrabScale;
        return GrabEdge(Current);
    }

    #endregion

    #region Checks with PhysicsCollider

    // Sets PhysicsCollider to player. Checks if there's any obstacles
    public bool SpaceOnPlayer(Vector3 ColliderSize)
    {

        _PhysicsCollider.transform.localPosition = new Vector3(0, 0.35f, -0.15f);
        _PhysicsCollider.transform.localScale = ColliderSize;

        bool Free1 = !CheckSphere(_PhysicsCollider, Vector3.zero, ObstacleLayer);

        _PhysicsCollider.transform.localPosition = new Vector3(0, 1.75f, -0.15f);

        bool Free2 = !CheckSphere(_PhysicsCollider, Vector3.zero, ObstacleLayer);

        return Free1 & Free2;
    }

    // Sets PhysicsCollider according to edge. Checks if there's any obstacles
    public bool SpaceOnEdge(Edge edge, float SidePosition)
    {

        // calculate ghost pos and rot
        bool Gradual = IsGradual(edge);
        _PlayerGhost.transform.rotation = RotationOnEdge(Gradual, edge);
        _PlayerGhost.transform.position = PositionOnEdge(Gradual, SidePosition, edge);

        // Switch _PhysicsCollider to be child of _PlayerGhost
        _PhysicsCollider.transform.parent = _PlayerGhost.transform;

        // Check space
        bool IsSpace = SpaceOnPlayer(new Vector3(0.8f, 0.8f, 0.8f));

        // Switch _PhysicsCollider parent back to ClimbingBehaviour
        _PhysicsCollider.transform.parent = this.transform;

        return IsSpace;
    }

    // Sets PhysicsCollider on top of edge and checks if there's any obstacles
    public bool SpaceOnTopOfEdge(Edge edge)
    {
        // calculate ghost pos and rot
        ResetPlayerGhost();
        Vector3 FactorY = edge.TransformToVectorInWorld(edge.UpDirection, false) * (_EdgeGrabDifferenceY * -1.5f);
        Vector3 FactorZ = edge.TransformToVectorInWorld(edge.ForwardDirection, false) * (_EdgeGrabDifferenceZ * -1.5f);
        _PlayerGhost.transform.position += FactorY + FactorZ;

        // Switch _PhysicsCollider to be child of _PlayerGhost
        _PhysicsCollider.transform.parent = _PlayerGhost.transform;

        // Check space
        bool IsSpace = SpaceOnPlayer(new Vector3(0.75f, 0.75f, 0.75f));

        // Switch _PhysicsCollider parent back to ClimbingBehaviour
        _PhysicsCollider.transform.parent = this.transform;

        return IsSpace;
    }

    #endregion

    #endregion

    #region Interacting with Edge

    #region Positions

    // Calculates player's position in the local space of Edge and returns difference from the center of edge in SideDirection
    public float SidePositionOnEdge(Transform obj, Edge edge, bool Clamp)
    {
        float SidePos = 0;

        // transform obj to local space of edge
        Vector3 ObjInEdgeSpace = edge.transform.InverseTransformPoint(new Vector3(obj.position.x, edge.transform.position.y, obj.position.z));

        if (edge.VectorDirectionX(edge.SideDirection))
        {
            SidePos = ObjInEdgeSpace.x * edge.TransformToVector(edge.SideDirection).x;
        } else if (edge.VectorDirectionY(edge.SideDirection))
        {
            SidePos = ObjInEdgeSpace.y * edge.TransformToVector(edge.SideDirection).y;
        } else if(edge.VectorDirectioZ(edge.SideDirection))
        {
            SidePos = ObjInEdgeSpace.z * edge.TransformToVector(edge.SideDirection).z;
        }

        if (Clamp)
        {
            return Mathf.Clamp(SidePos, -0.5f, 0.5f);
        } else
        {
            return SidePos;
        }
    }

    // Calculates real world point by moving center of given edge in SideDirection with given amount
    public Vector3 WorldPointOnEdge(float SidePos, Edge edge)
    {
        Vector3 Point = new Vector3(0, 0, 0);
        Point += edge.TransformToVector(edge.SideDirection) * SidePos;
        return edge.transform.TransformPoint(Point);
    }

    // Sets player's position and rotation according to the edge
    public void SetPlayerToEdge(float SidePos, Edge edge)
    {
        // if edge's angle is 0-89
        bool Gradual = IsGradual(edge);

        // Set rotation
        Player.transform.rotation = RotationOnEdge(Gradual, edge);

        // Set position
        Player.transform.position = PositionOnEdge(Gradual, SidePos, edge);
    }

    #endregion

    #region Finding best Edge

    // Finds best edge in the list. Returns false if edge not found
    private bool FindBestEdge(List<Edge> Edges)
    {

        for(int i = 0; i < Edges.Count; i++)
        {

            // Check player's position on SideDirection of Edge
            float SidePos = SidePositionOnEdge(Player.transform, Edges[i], false);

            if(Mathf.Abs(SidePos) < 0.5f)
            {

                // Check player's position on ForwardDirection of Edge
                Vector3 PlayerPositionOnEdgeSpace = Edges[i].transform.InverseTransformPoint(Player.transform.position);
                float Difference = 0;

                if (Edges[i].VectorDirectionX(Edges[i].ForwardDirection))
                {
                    Difference = PlayerPositionOnEdgeSpace.x * Edges[i].TransformToVector(Edges[i].ForwardDirection).x;
                }
                else if (Edges[i].VectorDirectionY(Edges[i].ForwardDirection))
                {
                    Difference = PlayerPositionOnEdgeSpace.y * Edges[i].TransformToVector(Edges[i].ForwardDirection).y;
                }
                else
                {
                    Difference = PlayerPositionOnEdgeSpace.z * Edges[i].TransformToVector(Edges[i].ForwardDirection).z;
                }

                // If player is between the start and end points of edge and inside the edge object, don't pick that edge
                if (Difference > 0)
                {
                    continue;
                }
            }

            ChangeMode(new ModeOnEdge(this, Edges[i]));
            return true;
        }

        return false;
    }
    

    #endregion

    #region Helpers

    // Is edge gradual (angle 0-89)
    public bool IsGradual(Edge edge)
    {
        return edge.TransformToVectorInWorld(edge.ForwardDirection, false).y < 0;
    }

    // Calculates rotation for object that hangs from edge
    public Quaternion RotationOnEdge(bool Gradual, Edge edge)
    {
        return Quaternion.LookRotation(edge.TransformToVectorInWorld(edge.ForwardDirection, !Gradual));
    }

    // Calculates position on the edge
    public Vector3 PositionOnEdge(bool Gradual, float SidePos, Edge edge)
    {
        // Y factor
        Vector3 FactorY;
        if (Gradual)
        {
            FactorY = edge.TransformToVectorInWorld(edge.UpDirection, false) * _EdgeGrabDifferenceY;
        }
        else
        {
            FactorY = Vector3.up * _EdgeGrabDifferenceY;
        }

        // Z factor
        Vector3 FactorZ = edge.TransformToVectorInWorld(edge.ForwardDirection, !Gradual) * _EdgeGrabDifferenceZ;

        return WorldPointOnEdge(SidePos, edge) + FactorY + FactorZ;
    }

    // Sets position and rotation of PlayerGhost to match Player
    private void ResetPlayerGhost()
    {
        _PlayerGhost.transform.position = Player.transform.position;
        _PlayerGhost.transform.rotation = Player.transform.rotation;
        _PlayerGhost.transform.localScale.Set(1, 1, 1);
    }

    #endregion

    #endregion

}
