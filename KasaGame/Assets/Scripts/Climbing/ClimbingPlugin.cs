using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class ClimbingPlugin : MonoBehaviour {

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

    #endregion

    #region Climbing Settings
    [Header("Climbing Settings")]

    // Layer for Obstacles
    [SerializeField]
    private LayerMask _ObstacleLayer;
    public LayerMask ObstacleLayer
    {
        get { return _ObstacleLayer; }
    }

    // Layer for EdgeFrames
    [SerializeField]
    private LayerMask _FrameLayer;

    // Layer for Edges
    [SerializeField]
    private LayerMask _EdgeLayer;
    public LayerMask EdgeLayer
    {
        get { return _EdgeLayer; }
    }

    // Maximum allowed gradient of Edge
    [SerializeField]
    private float _MaxGradientEdge;
    public float MaxGradientEdge
    {
        get { return _MaxGradientEdge; }
    }

    // Maximum allowed gradient of Facing
    [SerializeField]
    private float _MaxGradientFacing;
    public float MaxGradientFacing
    {
        get { return _MaxGradientFacing; }
    }

    #endregion

    #region Collider Detectors
    [Header("Collider Detectors")]

    // Used to check if player can grab a ledge
    [SerializeField]
    private SphereCollider _GrabCollider;
    public SphereCollider GrabCollider
    {
        get { return _GrabCollider; }
    }

    // Physics Collider, checks that player doesn't go inside things
    [SerializeField]
    private SphereCollider _PhysicsCollider;
    public SphereCollider PhysicsCollider
    {
        get { return _PhysicsCollider; }
    }

    // Used to check if player is standing on the edge
    [SerializeField]
    private SphereCollider _FeetCollider;
    public SphereCollider FeetCollider
    {
        get { return _FeetCollider; }
    }

    // Used to check if there's no obstacles in front of player's feet
    [SerializeField]
    private SphereCollider _EmptyCollider;
    public SphereCollider EmptyCollider
    {
        get { return _EmptyCollider; }
    }

    // Checks if there's enough room above player
    [SerializeField]
    private SphereCollider _RoomUpCollider;
    public SphereCollider RoomUpCollider
    {
        get { return _RoomUpCollider; }
    }

    // Checks if there's enough room below player
    [SerializeField]
    private SphereCollider _RoomDownCollider;
    public SphereCollider RoomDownCollider
    {
        get { return _RoomDownCollider; }
    }


    #endregion

    #region StateOfClimbing

    // Current state of player
    private StateOfClimbing _CurrentState;

    // Temporary list of Frames nearby
    private Collider[] _Frames;
    public Collider[] Frames
    {
        get { return _Frames; }
    }

    // Temporary delay added to prevent player from grabbing same Ledge right after leaving it
    private float _GrabDelay;
    public float GrabDelay
    {
        get { return _GrabDelay; }
        set { _GrabDelay = value; }
    }

    #endregion

    #endregion

    #region Initialization

    // Use this for initialization
    void Start () {
        transform.localPosition = new Vector3(0, 0, 0);
        FindPlayer();
        _CurrentState = new StateOnAir(this);
	}

    // Finds player character and scripts attached to it
    private void FindPlayer()
    {

        // find PlayerCharacter if missing
        if(_Player == null)
        {
            _Player = transform.parent.gameObject;
            if(_Player == null)
            {
                Debug.LogError("Player not found");
            }
        }

        // Find VThirdPersonController if missing
        if(_VController == null)
        {
            _VController = _Player.GetComponent<vThirdPersonController>();
            if(_VController == null)
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
    }

    #endregion

    // Update is called once per frame
    void Update () {

        // Find Frames nearby and update their Edges
        _Frames = Physics.OverlapSphere(transform.position, 2.5f, _FrameLayer);
        for (int i = 0; i < Frames.Length; i++)
        {
            Frames[i].GetComponent<FrameBehaviour>().UpdateEdges(_MaxGradientEdge, _MaxGradientFacing);
        }

        // Reduce GrabDelay if set
        if(GrabDelay > 0)
        {
            GrabDelay -= Time.deltaTime;
        }

        // Run state specific methods
        _CurrentState.RunState();

        // Disable Edges of found Frames
        for (int i = 0; i < Frames.Length; i++)
        {
            Frames[i].GetComponent<FrameBehaviour>().DisableEdges();
        }
    }

    // Enables or Disables default scripts that control the player
    public void EnableDefaultControllingSystem(bool enable)
    {
        _VController.enabled = enable;
        _VInput.enabled = enable;
        _Rigidbody.isKinematic = !enable;

        if (!enable)
        {
            _VController.isJumping = false;
        }
    }

    #region Collider checks

    // Performs Physics.CheckSphere with given parameters and returns result
    public bool CheckSphere(SphereCollider collider, Vector3 Offset, LayerMask layer)
    {
        return Physics.CheckSphere(collider.bounds.center + Offset, collider.bounds.size.y / 2, layer);
    }

    // Performs Physics.OverlapSphere with given parameters and returns result
    public Collider[] OverlapSphere(SphereCollider collider, LayerMask layer)
    {
        return Physics.OverlapSphere(collider.bounds.center, collider.bounds.size.y / 2, layer);
    }

    #endregion

    #region State Methods

    // Calls ExitState() of current state and EnterState() of new state
    public void ChangeState(StateOfClimbing state)
    {
        _CurrentState.ExitState();
        _CurrentState = state;
        _CurrentState.EnterState();
    }

    #endregion

}
