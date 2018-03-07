using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class ClimbingController : MonoBehaviour
{

    #region Variables

    #region Characters and Scripts

    // Player Character
    [SerializeField]
    private GameObject _PlayerCharacter;
    public GameObject PlayerCharacter
    {
        get { return _PlayerCharacter; }
    }

    // Camera
    [SerializeField]
    private GameObject _Camera;
    public GameObject Camera
    {
        get { return _Camera; }
    }

    // Reference to player controller script
    private vThirdPersonController _V3PC;
    public vThirdPersonController V3PC
    {
        get { return _V3PC; }
    }

    #endregion

    #region Layers

    // Layer for Climbable objects
    [SerializeField]
    private LayerMask _ClimbingLayer;
    public LayerMask ClimbingLayer
    {
        get { return _ClimbingLayer; }
    }

    // Layer for Obstacle objects
    [SerializeField]
    private LayerMask _ObstacleLayer;
    public LayerMask ObstacleLayer
    {
        get { return _ObstacleLayer; }
    }

    #endregion

    #region State

    // State of the Player: is player hanging, climbing up etc
    private enum ClimbingState
    {
        ON_GROUND, ON_AIR, ON_LEDGE
    }

    // Player's current state, determines what actions are performed
    private ClimbingState _CurrentState;



    #endregion

    #region Collider Checks

    // Used to identify if there's any Climbable objects ahead
    [SerializeField]
    private SphereCollider _InitialGrab;
    public SphereCollider InitialGrab
    {
        get { return _InitialGrab; }
    }

    // Checks Climbable objects on the left side of InitialGrab
    [SerializeField]
    private SphereCollider _LeftGrab;
    public SphereCollider LeftGrab
    {
        get { return _LeftGrab; }
    }

    // Checks Climbable objects on the right side of InitialGrab
    [SerializeField]
    private SphereCollider _RightGrab;
    public SphereCollider RightGrab
    {
        get { return _RightGrab; }
    }

    // Tracks player's rotation on left
    [SerializeField]
    private SphereCollider _LeftOutCheck;

    // Tracks player's rotation on left
    [SerializeField]
    private SphereCollider _LeftInCheck;

    // Tracks player's rotation on right
    [SerializeField]
    private SphereCollider _RightOutCheck;

    // Tracks player's rotation on right
    [SerializeField]
    private SphereCollider _RightInCheck;

    // Tracks player's position ++
    [SerializeField]
    private SphereCollider _DistanceInCheck;

    // Tracks player's position --
    [SerializeField]
    private SphereCollider _DistanceOutCheck;


    // Grab Point
    [SerializeField]
    private GameObject _GrabPoint;

    #endregion

    #region Temporary References

    // First Position of Ledge
    private Vector3 _GrabPosition;

    // Found Ledge
    private GameObject _Ledge;

    // Delay for grabbing after letting go
    float _InputDelay;

    #endregion

    #region Booleans

    // whether grab point is set or not
    private bool _GrabPointSet;

    #endregion

    #endregion


    #region Start and Update

    // Use this for initialization
    void Start()
    {

        // if PlayerCharacter is not set manually, set parent as PlayerCharacter
        if (_PlayerCharacter == null)
        {
            _PlayerCharacter = gameObject.transform.parent.gameObject;

            // if no parent was found, report error
            if (_PlayerCharacter == null)
            {
                Debug.LogError("Player Character not found");
            }
        }

        // gets vThirdPersonController from gameobject
        _V3PC = PlayerCharacter.GetComponent<vThirdPersonController>();

        // if vThirdPersonController was not found, repot error
        if (V3PC == null)
        {
            Debug.LogError("V Third Person Controller not found");
        }

        // Sets ON_GROUND as current state
        SetState(ClimbingState.ON_GROUND);
    }

    // Update is called once per frame
    void Update()
    {
        if (_InputDelay > 0)
        {
            _InputDelay -= Time.deltaTime;
            if (_InputDelay < 0)
            {
                _InputDelay = 0;
            }
        }

        // run state specific methods
        switch (_CurrentState)
        {
            case ClimbingState.ON_GROUND:
                RunOnGroundState();
                break;
            case ClimbingState.ON_AIR:
                RunOnAirState();
                break;
            case ClimbingState.ON_LEDGE:
                RunOnLedgeState();
                break;
        }

    }

    #endregion

    #region State Methods

    // Sets current state and runs state initialization method
    private void SetState(ClimbingState state)
    {
        _CurrentState = state;

        // run activation method
        switch (_CurrentState)
        {
            case ClimbingState.ON_GROUND:
                InitializeOnGroundState();
                break;
            case ClimbingState.ON_AIR:
                InitializeOnAirState();
                break;
            case ClimbingState.ON_LEDGE:
                InitializeOnLedgeState();
                break;
        }
    }

    // Run when ON_GROUND is set as current state
    private void InitializeOnGroundState()
    {
        // enable normal inputs, animations and gravity
        UseDefaultController(true);
        _GrabPointSet = false;
        _InputDelay = 0.25f;
    }

    // Actions in ON_GROUND state
    private void RunOnGroundState()
    {

        // If player is on air, switch to ON_AIR state instead
        if (!V3PC.isGrounded)
        {
            SetState(ClimbingState.ON_AIR);
            return;
        }


    }

    // Run when ON_AIR is set as current state
    private void InitializeOnAirState()
    {
        UseDefaultController(true);
        _GrabPointSet = false;
        _InputDelay = 0.1f;
    }

    // Actions in ON_AIR state
    private void RunOnAirState()
    {

        // If player is no more on air, return to ON_GROUND state
        if (V3PC.isGrounded)
        {
            SetState(ClimbingState.ON_GROUND);
            return;
        }

        // Check whether player holds grab button or not
        if (!Input.GetMouseButton(0))
        {
            return;
        }

        //
        if (_InputDelay > 0)
        {
            return;
        }

        // Check if there's actual grab point. Set _Ledge and _InitialGrabPosition
        if (!GrabPointFound(30))
        {
            return;
        }

        // if there's no continuous ledge, don't grab from ledge
        if (!LedgeFound(_LeftGrab, 25) || !LedgeFound(_RightGrab, 25))
        {
            return;
        }

        // Set Grab Point
        _GrabPoint.transform.position = _GrabPosition;
        Quaternion rotation = Quaternion.LookRotation(PlayerCharacter.transform.TransformDirection(Vector3.forward));
        _GrabPoint.transform.rotation = rotation;
        _GrabPointSet = true;
        _GrabPoint.transform.parent = _Ledge.transform;

        SetState(ClimbingState.ON_LEDGE);
    }

    // Run when ON_LEDGE is set as current state
    private void InitializeOnLedgeState()
    {
        // disable normal inputs, animations and gravity
        UseDefaultController(false);
        _InputDelay = 0.4f;
    }

    // Actions in ON_LEDGE state
    private void RunOnLedgeState()
    {
        // If player lets go from grab button, player starts falling
        if (!Input.GetMouseButton(0))
        {
            SetState(ClimbingState.ON_AIR);
            return;
        }

        // Set player to the position and rotation of _GrabPoint
        PlayerCharacter.transform.position = _GrabPoint.transform.position + _GrabPoint.transform.TransformDirection(Vector3.forward).normalized * -0.4f + _GrabPoint.transform.TransformDirection(Vector3.up).normalized * -2;
        PlayerCharacter.transform.rotation = _GrabPoint.transform.rotation;

        // Set Camera position
       

        // Check if there's a ledge to grab from anymore
        if (!GrabPointFound(20))
        {
            Debug.Log("CLIMB");
            SetState(ClimbingState.ON_AIR);
            return;
        }
        Debug.Log("CLIMB2");

        // Check inputs
        if (_InputDelay > 0)
        {
            return;
        }
        if (Input.GetKey(KeyCode.A))
        {
            HangSideways(-1f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            HangSideways(1f);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            if (StandUp())
            {
                return;
            }
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            SetState(ClimbingState.ON_AIR);
            V3PC.isJumping = true;
        }

    }

    #endregion

    #region Action Checks

    // returns true if given collider overlaps with any collider in given layer
    private bool CheckSphere(SphereCollider collider, Vector3 Offset, LayerMask layer)
    {
        return Physics.CheckSphere(collider.bounds.center + Offset, collider.bounds.size.y / 2, layer);
    }

    // returns list of colliders that overlap given collider in given layer
    private Collider[] OverlapsSphere(SphereCollider collider, Vector3 Offset, LayerMask layer)
    {
        return Physics.OverlapSphere(collider.bounds.center + Offset, collider.bounds.size.y / 2, layer);
    }

    // Check if there's Climbable objects ahead, and if there's ledge
    private bool LedgeFound(SphereCollider Collider, int Iterations)
    {

        // Check if ledge is near
        for (int i = 0; i < Iterations; i++)
        {
            // If no objects found at starting point, there's no Climbable object
            if (i == 0)
            {
                if (!CheckSphere(Collider, Vector3.zero, ClimbingLayer))
                {
                    return false;
                }
                else
                {
                    continue;
                }
            }

            // If object ends here, there's a ledge
            if (!CheckSphere(Collider, new Vector3(0, i * 0.01f, 0), ClimbingLayer))
            {
                return true;
            }
        }

        return false;
    }

    // Finds first grab position if there's any ledge ahead
    private bool GrabPointFound(int Iterations)
    {
        // Check if ledge is near
        for (int i = 0; i < Iterations; i++)
        {
            // If no objects found at starting point, there's no Climbable object
            if (i == 0)
            {
                Collider[] Colliders = OverlapsSphere(InitialGrab, Vector3.zero, ClimbingLayer);
                if (Colliders.Length == 0)
                {
                    return false;
                }
                else
                {
                    _Ledge = Colliders[0].gameObject;
                    continue;
                }
            }

            // If object ends here, there's a ledge
            if (!CheckSphere(InitialGrab, new Vector3(0, i * 0.01f, 0), ClimbingLayer))
            {
                _GrabPosition = InitialGrab.bounds.center + new Vector3(0, (i - 1) * 0.01f - InitialGrab.bounds.size.y / 2, 0);
                return true;
            }
        }

        return false;
    }

    // returns true if standing up was succesful
    private bool StandUp()
    {
        if (!Physics.CheckBox(_GrabPoint.transform.position + Vector3.up, new Vector3(0.25f, 0.8f, 0.1f)))
        {
            PlayerCharacter.transform.position = _GrabPoint.transform.position + Vector3.up * 0.5f + Vector3.forward * 0.1f;
            SetState(ClimbingState.ON_GROUND);
            return true;
        }
        return false;
    }

    // moves player sideways
    private void HangSideways(float moveSpeed)
    {
        AdjustLedgePosition();

        bool PreventMovement = false;
        _GrabPoint.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);


        if (!GrabPointFound(20))
        {
            PreventMovement = true;
        }

        if(PreventMovement)
        {
            _GrabPoint.transform.Translate(Vector3.right * -moveSpeed * Time.deltaTime);
        }
    }

    // Moves and rotates player so that he is better placed in the ledge
    private void AdjustLedgePosition()
    {

        bool LeftOut = !CheckSphere(_LeftOutCheck, Vector3.zero, ClimbingLayer);
        bool LeftIn = CheckSphere(_LeftInCheck, Vector3.zero, ClimbingLayer);
        bool RightOut = !CheckSphere(_RightOutCheck, Vector3.zero, ClimbingLayer);
        bool RightIn = CheckSphere(_RightInCheck, Vector3.zero, ClimbingLayer);
        bool DistanceIn = CheckSphere(_DistanceInCheck, Vector3.zero, ClimbingLayer);
        bool DistanceOut = !CheckSphere(_DistanceOutCheck, Vector3.zero, ClimbingLayer);

        float Angle = 180f * Time.deltaTime;
        Vector3 Forward = Vector3.forward * Time.deltaTime * 0.5f;

        if(!RightOut && LeftOut)
        {
            _GrabPoint.transform.Rotate(new Vector3(0, Angle, 0));
        }

        if (!LeftOut && RightOut)
        {
            _GrabPoint.transform.Rotate(new Vector3(0, -Angle, 0));
        }

        if (!LeftIn && RightIn)
        {
            _GrabPoint.transform.Rotate(new Vector3(0, Angle, 0));
            _GrabPoint.transform.Translate(Forward);

            RightIn = CheckSphere(_RightInCheck, Vector3.zero, ClimbingLayer);
        }

        if (!LeftIn && !RightIn)
        {
            _GrabPoint.transform.Translate(Forward);
        }


        if (!RightIn && LeftIn)
        {
            _GrabPoint.transform.Rotate(new Vector3(0, -Angle, 0));
        }

        if(!DistanceIn)
        {
            _GrabPoint.transform.Translate(Forward);
        }

        if(!DistanceOut)
        {
            _GrabPoint.transform.Translate(-Forward);
        }

    }

    #endregion

    #region Miscellaneous

    // enables or disables scripts that control Physics and Inputs for PlayerCharacter
    private void UseDefaultController(bool enable)
    {
        PlayerCharacter.GetComponent<vThirdPersonInput>().enabled = enable;
        PlayerCharacter.GetComponent<Rigidbody>().isKinematic = !enable;
        PlayerCharacter.GetComponent<CapsuleCollider>().enabled = enable;
        if (!enable)
        {
            V3PC.isJumping = false;
        }
        V3PC.lockMovement = !enable;
    }

    #endregion

}
