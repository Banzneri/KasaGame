using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProClimber : MonoBehaviour {

    public CharacterController CharController;
    public ClimberController ClimbController;
    public Collider GrabCollider;
    public LayerMask ClimbingMask;

    // state
    private enum ClimberState
    {
        DEFAULT, ON_AIR, ON_EDGE
    }
    private ClimberState _CurrentState;

    // Edge
    private Edge _CurrentEdge;
    public Edge CurrentEdge
    {
        get { return _CurrentEdge; }
        set { _CurrentEdge = value; }
    }


	// Use this for initialization
	void Start () {
        _CurrentState = ClimberState.DEFAULT;
        _CurrentEdge = null;
	}
	
	// Update is called once per frame
	void Update () {
        RunState(_CurrentState);
	}

    // running state actions
    private void RunState(ClimberState state)
    {
        switch(state)
        {
            case ClimberState.DEFAULT:
                CharController.enabled = true;
                if (!CharController.isGrounded)
                {
                    _CurrentState = ClimberState.ON_AIR;
                }
                break;
            case ClimberState.ON_AIR:
                CharController.enabled = true;
                OnAir();
                break;
            case ClimberState.ON_EDGE:
                OnEdge();
                break;
        }
    }

    // ON_AIR state
    private void OnAir()
    {
        // if player is on ground, change state
        if(CharController.isGrounded)
        {
            _CurrentState = ClimberState.DEFAULT;
            return;
        }

        // If pressing Grab button
        if(Input.GetKey(KeyCode.Q))
        {
            // pick first found edge and connect to it
            Collider[] nearbyEdges = Physics.OverlapBox(GrabCollider.bounds.center, GrabCollider.bounds.extents, GrabCollider.transform.rotation, ClimbingMask);
            if(nearbyEdges.Length > 0)
            {
                _CurrentEdge = nearbyEdges[0].GetComponent<Edge>();
                _CurrentState = ClimberState.ON_EDGE;
                ConnectToEdge();
            }
        }
    }

    // Attach to the edge
    private void ConnectToEdge()
    {
        Quaternion rotation = Quaternion.LookRotation(CurrentEdge.transform.forward);
        CharController.gameObject.transform.rotation = rotation;

        CharController.gameObject.transform.position = CurrentEdge.transform.position;

        CharController.enabled = false;
    }

    // ON_EDGE state
    private void OnEdge()
    {
        // if player lets Grab button go, let go
        if(!Input.GetKey(KeyCode.Q))
        {
            _CurrentState = ClimberState.ON_AIR;
        }

        // remove gravity
        

    }

}
