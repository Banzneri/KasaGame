using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbAddOn : MonoBehaviour {

    // Character Controller of the player
    public CharacterController Controller;

    // Temporary additional controller, remove from final version
    public ClimberController TempController;

    // Recognizes edges
    private BoxCollider GrabCollider;

    // Layer for Edges
    public LayerMask ClimbingLayer;

    // Climbing stamina
    private float _Stamina;

    // The edge player is hanging from
    private Edge _CurrentEdge;

    // State of the Climber
    private enum ClimbState
    {
        OFF, ON_AIR, ON_EDGE, TRANSFORM_DOWN, TRANSFORM_UP, TRANSFORM_CORNER
    }
    private ClimbState _CurrentState;
    


    // Use this for initialization
    void Start () {

        // Find GrabCollider
        GrabCollider = GetComponent<BoxCollider>();
        if(GrabCollider == null)
        {
            gameObject.AddComponent<BoxCollider>();

        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}



}
